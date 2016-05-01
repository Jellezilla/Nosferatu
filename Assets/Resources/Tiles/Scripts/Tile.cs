using UnityEngine;
using System.Collections.Generic;

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

    private Vector3[] m_HumanSpawnPoints;
    private GameObject[] m_Humans;
    private Vector3[] m_TombSpawnPoints;
    private Dictionary<int,Stack<GameObject>> m_Tombstones;
    // Use this for initialization

    void OnEnable()
    {
        InitTile();
        ResetTile();
    }

    void OnDisable()
    {
        UnloadTile();
    }


    void InitTile()
    {
        if (!m_initTile)
        {
            m_initTile = true;
            m_Humans = new GameObject[m_HumanContainer.transform.childCount];
            m_Tombstones = new Dictionary<int, Stack<GameObject>>();

            for (int i = 0; i < m_TombstoneIndexs.Length; i++)
            {
                m_Tombstones.Add(i, new Stack<GameObject>());
            }

            m_HumanSpawnPoints = new Vector3[m_HumanContainer.transform.childCount];
            m_TombSpawnPoints = new Vector3[m_TombstonesContainer.transform.childCount]; 

            for (int i = 0; i < m_HumanContainer.transform.childCount; i++)
            {
                m_HumanSpawnPoints[i] = m_HumanContainer.transform.GetChild(i).gameObject.transform.position;
            }

            for (int i = 0; i < m_TombstonesContainer.transform.childCount; i++)
            {
                m_TombSpawnPoints[i] = m_TombstonesContainer.transform.GetChild(i).gameObject.transform.position;
            }


            //REMEMBER TO IMPLEMENT NEW TOMBSTONES
        }

    }

    void ResetTile()
    {
        for (int i = 0; i < m_HumanContainer.transform.childCount; i++)
        {
            m_HumanSpawnPoints[i] = m_HumanContainer.transform.GetChild(i).gameObject.transform.position;
        }

        for (int i = 0; i < m_TombstonesContainer.transform.childCount; i++)
        {
            m_TombSpawnPoints[i] = m_TombstonesContainer.transform.GetChild(i).gameObject.transform.position;
        }

        for (int i = 0; i < m_HumanSpawnPoints.Length; i++)
        {
            m_Humans[i] = GameController.Instance.ObjectPool.GrabObject(m_HumanIndex, m_HumanSpawnPoints[i], Quaternion.identity);
            m_Humans[i].transform.SetParent(m_HumanContainer.transform);
        }

        for (int i = 0; i < m_TombSpawnPoints.Length; i++)
        {
            int dictIndex = Random.Range(0, m_TombstoneIndexs.Length);
            int randomIndex = m_TombstoneIndexs[dictIndex];
            m_Tombstones[dictIndex].Push(GameController.Instance.ObjectPool.GrabObject(randomIndex, m_TombSpawnPoints[i], Quaternion.identity));
            m_Tombstones[dictIndex].Peek().transform.SetParent(m_TombstonesContainer.transform);
        }



    }

    void UnloadTile()
    {
        if (!GameController.isQuitting)
        {
            for (int i = 0; i < m_Humans.Length; i++)
            {
                m_Humans[i].transform.SetParent(null);
                GameController.Instance.ObjectPool.ReturnObject(m_HumanIndex, m_Humans[i]);
            }

            for (int i = 0; i < m_TombstoneIndexs.Length; i++)
            {
                for (int j = m_Tombstones[i].Count; j >0; j--)
                {
                    m_Tombstones[i].Peek().transform.SetParent(null);
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
