using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Tile : MonoBehaviour {

    private bool m_initTile;
    [SerializeField]
    private int m_HumanIndex;
    [SerializeField]
    private int[] m_TombstoneIndexs;
    [SerializeField]
    private GameObject m_HumanContainer;
    [SerializeField]
    private GameObject m_TombstonesContainer;

    private Transform[] m_HumanSpawnPoints;
    private GameObject[] m_Humans;
    private Transform[] m_TombSpawnPoints;
    private Dictionary<int,Stack<GameObject>> m_Tombstones;
    // Use this for initialization

    void Awake()
    {
        InitTile();
    }


    void InitTile()
    {
        if (!m_initTile)
        {
            m_initTile = true;
            m_Humans = new GameObject[m_HumanContainer.transform.childCount];
            m_Tombstones = new Dictionary<int, Stack<GameObject>>();
            m_HumanSpawnPoints = new Transform[m_HumanContainer.transform.childCount];
            m_TombSpawnPoints = new Transform[m_TombstonesContainer.transform.childCount];

            for (int i = 0; i < m_TombstoneIndexs.Length; i++)
            {
                m_Tombstones.Add(i, new Stack<GameObject>());
            }



            for (int i = 0; i < m_HumanContainer.transform.childCount; i++)
            {
                m_HumanSpawnPoints[i] = m_HumanContainer.transform.GetChild(i);
            }

            for (int i = 0; i < m_TombstonesContainer.transform.childCount; i++)
            {
                m_TombSpawnPoints[i] = m_TombstonesContainer.transform.GetChild(i);
            }

        }

    }

    public void ResetTile()
    {

        for (int i = 0; i < m_HumanSpawnPoints.Length; i++)
        {
            m_Humans[i] = GameController.Instance.ObjectPool.GrabObject(m_HumanIndex, m_HumanSpawnPoints[i].position, Quaternion.identity);
        }

        for (int i = 0; i < m_TombSpawnPoints.Length; i++)
        {
            int dictIndex = Random.Range(0, m_TombstoneIndexs.Length);
            int randomIndex = m_TombstoneIndexs[dictIndex];
            m_Tombstones[dictIndex].Push(GameController.Instance.ObjectPool.GrabObject(randomIndex, m_TombSpawnPoints[i].position));
        }



    }

    public void UnloadTile()
    {
        if (!GameController.isQuitting)
        {
            for (int i = 0; i < m_Humans.Length; i++)
            {
                GameController.Instance.ObjectPool.ReturnObject(m_HumanIndex, m_Humans[i]);
            }

            for (int i = 0; i < m_TombstoneIndexs.Length; i++)
            {
                for (int j = m_Tombstones[i].Count; j >0; j--)
                {
                    GameController.Instance.ObjectPool.ReturnObject(m_TombstoneIndexs[i], m_Tombstones[i].Pop());
                }
            }
        }
       
        /// add tombstone logic
    }


    // Update is called once per frame
    void Update () {
	
	}
}
