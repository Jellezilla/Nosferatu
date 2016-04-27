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
    private Dictionary<int,ListStack<GameObject>> m_UnUsedObjects;
    private Dictionary<int,ListStack<GameObject>> m_UsedObjects;
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
        m_UnUsedObjects = new Dictionary<int, ListStack<GameObject>>();
        m_UsedObjects = new Dictionary<int, ListStack<GameObject>>();
        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            m_UnUsedObjects.Add(i, new ListStack<GameObject>());
            m_UsedObjects.Add(i, new ListStack<GameObject>());
            

            for (int j = 0; j < m_NrOfInstances[i]; j++)
            {
                GameObject prefab = Instantiate(m_Prefabs[i]);
                prefab.SetActive(false);
                m_UnUsedObjects[i].Push(prefab);
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
        GameObject gameOb;
        if (m_UnUsedObjects[objectIndex].Count > 0)
        {
            gameOb = m_UnUsedObjects[objectIndex].Pop();
            gameOb.transform.position = position;
            gameOb.transform.rotation = rotation;
            m_UsedObjects[objectIndex].Push(gameOb);
            gameOb.SetActive(true);
            return gameOb;
        }
        else
        {
            gameOb = (GameObject)Instantiate(m_Prefabs[objectIndex],position,rotation);
            m_UsedObjects[objectIndex].Push(gameOb);
            return gameOb;

        }
    }

    /// <summary>
    /// Returns an object to the pool based on its index position
    /// </summary>
    /// <param name="objectIndex"></param>
    /// <param name="obj"></param>
    public void ReturnObject(int objectIndex,GameObject obj)
    {
        if (m_UsedObjects[objectIndex].Contains(obj) && obj!=null)
        {
            obj.SetActive(false);
            m_UsedObjects[objectIndex].Remove(obj);
            m_UnUsedObjects[objectIndex].Push(obj);
        }
    }

}