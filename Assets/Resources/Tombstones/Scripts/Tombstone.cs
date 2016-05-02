using UnityEngine;
using System.Collections.Generic;

public class Tombstone : MonoBehaviour {

    private Rigidbody[] m_childrenRbs;
    private Vector3[] m_childPositions;
    private Quaternion[] m_childRotations;
    private List<BoxCollider> m_childrenCols;
    private BoxCollider m_col;
    private bool m_init;
    private bool m_broken;
    // Use this for initialization

    void Awake()
    {
        Init();
    }

    void OnDisable()
    {
        Reset();
    }
    private void Init()
    {
        if (!m_init)
        {
            m_init = true;
            m_childrenRbs = new Rigidbody[transform.childCount];
            m_childPositions = new Vector3[transform.childCount];
            m_childRotations = new Quaternion[transform.childCount];
            m_childrenCols = new List<BoxCollider>();
            m_col = GetComponent<BoxCollider>();
            for (int i = 0; i < transform.childCount; i++)
            {
                m_childrenRbs[i] = transform.GetChild(i).GetComponent<Rigidbody>();
                m_childPositions[i] = m_childrenRbs[i].transform.localPosition;
                m_childRotations[i] = m_childrenRbs[i].transform.localRotation;
                BoxCollider[] boxCol = m_childrenRbs[i].GetComponents<BoxCollider>();
                for (int j = 0; j < boxCol.Length; j++)
                {
                    m_childrenCols.Add(boxCol[j]);
                }
            }
        }

    }

    private void Reset()
    {
        if (m_broken)
        {
            m_broken = false;

            for (int i = 0; i < m_childrenRbs.Length; i++)
            {
                m_childrenRbs[i].useGravity = false;
                m_childrenRbs[i].detectCollisions = false;
                m_childrenRbs[i].velocity = Vector3.zero;
                m_childrenRbs[i].angularVelocity = Vector3.zero;
                m_childrenRbs[i].transform.localRotation = m_childRotations[i];
                m_childrenRbs[i].transform.localPosition = m_childPositions[i];


            }

            for (int i = 0; i < m_childrenCols.Count; i++)
            {
                m_childrenCols[i].enabled = false;
            }


            m_col.enabled = true;
        }

    }




    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == Tags.player || collider.gameObject.tag == Tags.human)
        {
            m_col.enabled = false;
            m_broken = true;
            for (int i = 0; i < m_childrenCols.Count; i++)
            {
                m_childrenCols[i].enabled = true;
            }

            for (int i = 0; i < m_childrenRbs.Length; i++)
            {
                m_childrenRbs[i].detectCollisions = true;
                m_childrenRbs[i].useGravity = true;


            }


        }
    }

}
