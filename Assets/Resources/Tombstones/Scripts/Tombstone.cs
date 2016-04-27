using UnityEngine;
using System.Collections.Generic;

public class Tombstone : MonoBehaviour {

    private Rigidbody[] m_childrenRbs;
    private Vector3[] m_childPositions;
    private Quaternion[] m_childRotations;
    private List<BoxCollider> m_childrenCols;
    private BoxCollider m_col;
	// Use this for initialization
	void Awake () {

        Init();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Init()
    {
        m_childrenRbs = new Rigidbody[transform.childCount];
        m_childPositions = new Vector3[transform.childCount];
        m_childRotations = new Quaternion[transform.childCount];
        m_childrenCols = new List<BoxCollider>();
        m_col = GetComponent<BoxCollider>();
        for (int i = 0; i < transform.childCount; i++)
        {
            m_childrenRbs[i] = transform.GetChild(i).GetComponent<Rigidbody>();
            m_childPositions[i] = m_childrenRbs[i].transform.position;
            m_childRotations[i] = m_childrenRbs[i].transform.rotation;
            BoxCollider[] boxCol = m_childrenRbs[i].GetComponents<BoxCollider>();
            for (int j = 0; j < boxCol.Length; j++)
            {
                m_childrenCols.Add(boxCol[j]);
            }
        }
    }

    void Reset()
    {


        for (int i = 0; i < m_childrenRbs.Length; i++)
        {
            m_childrenRbs[i].isKinematic = true;

        }

        for (int i = 0; i < m_childrenCols.Count; i++)
        {
            m_childrenCols[i].enabled = false;
        }

        m_col.enabled = true;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tags.player || collision.gameObject.tag == Tags.human)
        {
            m_col.enabled = false;

            for (int i = 0; i < m_childrenCols.Count; i++)
            {
                m_childrenCols[i].enabled = true;
            }

            for (int i = 0; i < m_childrenRbs.Length; i++)
            {
                m_childrenRbs[i].isKinematic = false;

            }


        }
    }

}
