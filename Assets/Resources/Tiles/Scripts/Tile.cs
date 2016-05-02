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
    private List<GameObject>[] m_Tombstones;
    WaitForEndOfFrame m_wait = new WaitForEndOfFrame();
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
            m_Humans = new GameObject[m_HumanContainer.transform.childCount];
            m_Tombstones = new List<GameObject>[m_TombstoneIndexs.Length];

            m_HumanSpawnPoints = new Transform[m_HumanContainer.transform.childCount];
            m_TombSpawnPoints = new Transform[m_TombstonesContainer.transform.childCount];

            for (int i = 0; i < m_TombstoneIndexs.Length; i++)
            {
                m_Tombstones[i] = new List<GameObject>(0);
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
        gameObject.SetActive(true);
        StartCoroutine(TileLoaderRoutine());
    }
    public void TileUnloader()
    {
        StartCoroutine(TileUnloaderRoutine());
    }

    IEnumerator TileLoaderRoutine()
    {

        int humanCounter = m_HumanSpawnPoints.Length - 1;
        int tombstoneCounter = m_TombSpawnPoints.Length - 1;

        while (!m_loaded)
        {
            if (humanCounter > 0)
            {
                m_Humans[humanCounter] = GameController.Instance.ObjectPool.GrabObject(m_HumanIndex, m_HumanSpawnPoints[humanCounter].position, Quaternion.identity);
                humanCounter--;
            }

            if (tombstoneCounter > 0)
            {
                for (int i = 0; i < m_TombstoneIndexs.Length; i++)
                {

                        m_Tombstones[i].Add(GameController.Instance.ObjectPool.GrabObject(m_TombstoneIndexs[i],m_TombSpawnPoints[tombstoneCounter].position));
                        tombstoneCounter--;
                }
            }
            yield return m_wait;
        }
        m_loaded = true;
    }
    IEnumerator TileUnloaderRoutine()
    {
        int humanCounter = m_HumanSpawnPoints.Length-1;
        int tombstoneCounter = m_TombSpawnPoints.Length-1;
        while (m_loaded)
        {
            //m_Humans[humanCounter]
            if (humanCounter > 0)
            {
                GameController.Instance.ObjectPool.ReturnObject(m_HumanIndex, m_Humans[humanCounter]);
                humanCounter--;
            }

            if (tombstoneCounter > 0)
            {
                for (int i = 0; i < m_TombstoneIndexs.Length; i++)
                {
                    if (m_Tombstones[i].Count > 0)
                    {
                        GameController.Instance.ObjectPool.ReturnObject(m_TombstoneIndexs[i], m_Tombstones[i][0]);
                        m_Tombstones[i].Remove(m_Tombstones[i][0]);
                        tombstoneCounter--;
                    }

                }
            }
            yield return m_wait;
        }
        m_loaded = false;
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
