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
    private HumanController[] m_Humans;
    private Transform[] m_TombSpawnPoints;
    private List<Tombstone>[] m_Tombstones;
    WaitForFixedUpdate m_wait = new WaitForFixedUpdate();
    private bool m_loaded;
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
           m_Humans = new HumanController[m_HumanContainer.transform.childCount];
           m_Tombstones = new List<Tombstone>[m_TombstoneIndexs.Length];

            m_HumanSpawnPoints = new Transform[m_HumanContainer.transform.childCount];
            m_TombSpawnPoints = new Transform[m_TombstonesContainer.transform.childCount];

            for (int i = 0; i < m_TombstoneIndexs.Length; i++)
            {
                m_Tombstones[i] = new List<Tombstone>(0);
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

    public void TileLoader()
    {
        Debug.Log(m_TombSpawnPoints.Length);
        gameObject.SetActive(true);
        for (int i = 0; i < m_HumanSpawnPoints.Length; i++)
        {
           m_Humans[i]  = GameController.Instance.ObjectPool.GrabObject(m_HumanIndex, m_HumanSpawnPoints[i].position, Quaternion.identity).GetComponent<HumanController>();
        }

        for(int i = 0; i< m_TombSpawnPoints.Length; i=i+m_Tombstones.Length)
        {
            for (int j = 0; j < m_Tombstones.Length; j++)
            {
                if (i == m_TombSpawnPoints.Length - 1)
                {
                    m_Tombstones[j].Add(GameController.Instance.ObjectPool.GrabObject(m_TombstoneIndexs[j], m_TombSpawnPoints[i].position).GetComponent<Tombstone>());
                    break;
                }
                else
                {
                    m_Tombstones[j].Add(GameController.Instance.ObjectPool.GrabObject(m_TombstoneIndexs[j], m_TombSpawnPoints[i + j].position).GetComponent<Tombstone>());
                }

            }
        }



        }
    public void TileUnloader()
    {
        for (int i = 0; i < m_Humans.Length; i++)
        {
            m_Humans[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < m_Tombstones.Length; i++)
        {
            for (int j = 0; j < m_Tombstones[i].Count; j++)
            {
                m_Tombstones[i][j].gameObject.SetActive(false);
                
            }
            m_Tombstones[i].Clear();
        }

        gameObject.SetActive(false);
    }

    IEnumerator TileLoaderRoutine()
    {

        int humanCounter = m_HumanSpawnPoints.Length - 1;
        int tombstoneCounter = m_TombSpawnPoints.Length - 1;

        while (!m_loaded)
        {
            if (humanCounter > 0)
            {
                m_Humans[humanCounter] = GameController.Instance.ObjectPool.GrabObject(m_HumanIndex, m_HumanSpawnPoints[humanCounter].position, Quaternion.identity).GetComponent<HumanController>();
                humanCounter--;
            }

            if (tombstoneCounter > 0)
            {
                for (int i = 0; i < m_TombstoneIndexs.Length; i++)
                {

                     m_Tombstones[i].Add(GameController.Instance.ObjectPool.GrabObject(m_TombstoneIndexs[i],m_TombSpawnPoints[tombstoneCounter].position).GetComponent<Tombstone>());
                        tombstoneCounter--;
                }
            }
            yield return m_wait;
        }
        m_loaded = true;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
