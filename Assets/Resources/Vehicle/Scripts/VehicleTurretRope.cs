using UnityEngine;
using System.Collections.Generic;

public class VehicleTurretRope : MonoBehaviour {

    [SerializeField]
    private GameObject m_Emitter;
    private GameObject m_Target;
    [SerializeField]
    private Material m_RopeMaterial;
    [SerializeField]
    private float m_ropeWidth = 0.5f;
    private Vector3[] m_RopeSegments;
    [Range(4,2)][SerializeField][Tooltip("How many segments per metric unit. 1 -> 1")]
    private int m_segmentRate = 1;
    private Vector3 m_segmentSeparator;
    private int m_segments;
    private LineRenderer m_ropeRenderer;



    void Awake () {


        m_ropeRenderer = m_Emitter.AddComponent<LineRenderer>();
        m_ropeRenderer.material = m_RopeMaterial;
        m_ropeRenderer.SetWidth(m_ropeWidth, m_ropeWidth);
        m_ropeRenderer.enabled = false;


    }
   
    private void ComputeRope()
    {
        m_segments = (int)(Vector3.Distance(m_Emitter.transform.position, m_Target.transform.position) * m_segmentRate);
        m_ropeRenderer.SetVertexCount(m_segments);
        Vector3 heading = m_Target.transform.position - m_Emitter.transform.position;
        m_segmentSeparator = heading / (m_segments - 1);
        m_RopeSegments = new Vector3[m_segments]; //each time populate the array with nulls
        for (int i = 0; i < m_segments; i++)
        {
            if (i == 0)
            {
                m_RopeSegments[i] = m_Emitter.transform.position; // base
            }
            else if (i == m_segments - 1)
            {
                m_RopeSegments[i] = m_Target.transform.position; // head

            }
            else // body
            {
                Vector3 segmentPos = (m_segmentSeparator * i) + m_Emitter.transform.position;
                m_RopeSegments[i] = segmentPos;

            }
            

        }



       
    }

    public void CreateRope(GameObject ropeTarget)
    {
        m_Target = ropeTarget;
        m_ropeRenderer.enabled = true;
        ComputeRope();


    }

    public void RopeUpdate()
    {
        if (m_Target != null)
        {
            ComputeRope();
            for (int idx = 0; idx < m_RopeSegments.Length; idx++)
            {
                m_ropeRenderer.SetPosition(idx, m_RopeSegments[idx]);
            }
        }
    }

    public void DestroyRope()
    {
        m_Target = null;
        m_ropeRenderer.enabled = false;
    }
	// Update is called once per frame
	void LateUpdate () {

        RopeUpdate();
	}
}

