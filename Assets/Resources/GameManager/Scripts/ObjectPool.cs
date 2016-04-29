using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Object Pooling Class
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Prefabs;
    [SerializeField]
    private int[] m_NrOfInstances;
  //  private Dictionary<int,ListStack<GameObject>> m_UnUsedObjects;
    private Dictionary<int,List<GameObject>> m_pools;
    private GameObject testObj;



    void Awake()
    {
        InitPool();
    }

    /// <summary>
    /// Init pool and populate with prefab instances
    /// </summary>
    void InitPool()
    {
     //   m_UnUsedObjects = new Dictionary<int, ListStack<GameObject>>();
        m_pools = new Dictionary<int, List<GameObject>>();
        for (int i = 0; i < m_Prefabs.Length; i++)
        {
          //  m_UnUsedObjects.Add(i, new ListStack<GameObject>());
            m_pools.Add(i, new List<GameObject>());
            

            for (int j = 0; j < m_NrOfInstances[i]; j++)
            {
                GameObject prefab = Instantiate(m_Prefabs[i]);
                prefab.SetActive(false);
               m_pools[i].Add(prefab);
            }
        }

        
    }

    /// <summary>
    /// Returns a pooled object corresponding to the given index.
    /// </summary>
    /// <param name="objectIndex"> position within the list of objects</param>
    /// <param name="position"> object position </param>
    /// <param name="rotation"> object rotation </param>
    /// <returns></returns>
    public GameObject GrabObject(int objectIndex, Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < m_pools[objectIndex].Count; i++)
        {
            if (!m_pools[objectIndex][i].activeSelf)
            {
                m_pools[objectIndex][i].transform.position = position;
                m_pools[objectIndex][i].transform.rotation = rotation;
                m_pools[objectIndex][i].SetActive(true);

                return m_pools[objectIndex][i];

            }
        }

        /// no objects are available
        m_pools[objectIndex].Add(Instantiate(m_Prefabs[objectIndex]));
        m_pools[objectIndex][m_pools[objectIndex].Count - 1].transform.position = position;
        m_pools[objectIndex][m_pools[objectIndex].Count - 1].transform.rotation = rotation;
        return m_pools[objectIndex][m_pools[objectIndex].Count - 1];
    }

    /// <summary>
    /// Returns an object to the pool based on its index position
    /// </summary>
    /// <param name="objectIndex"></param>
    /// <param name="obj"></param>
    public void ReturnObject(int objectIndex,GameObject obj)
    {
        if (m_pools[objectIndex].Contains(obj) && obj!=null)
        {
            obj.SetActive(false);
        }
    }

}