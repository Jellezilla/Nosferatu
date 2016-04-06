using UnityEngine;
using System.Collections.Generic;

public class VehicleTurretRope : MonoBehaviour {

    [SerializeField]
    private GameObject m_Emitter;
    private GameObject m_Target;
    private Vector3 m_OldTargetPos;
    [SerializeField]
    private Material m_RopeMaterial;
    [SerializeField]
    private float m_ropeWidth = 0.5f;
    private float m_RopeSDrag = 0.1f;  
    private float m_RopeSMass = 0.1f;                    
    private float m_RopeSColliderRadius = 0.15f;
    [SerializeField]
    private float m_RopeBreakForce;
    [SerializeField]
    private float m_RopeBreakTorque;
    private Vector3[] m_RopeSegments;
    private GameObject[] m_RopeJoints;
    [Range(1,2)][SerializeField][Tooltip("How many segments per metric unit. 1 -> 1")]
    private int m_segmentRate = 1;
    private Vector3 m_segmentSeparator;
    [SerializeField]
    private int m_PrecachedSegments;
    private int m_segments;
    private LineRenderer m_ropeRenderer;
    private bool m_isMaxDist;

    public bool m_isRope
    {
        get;
        private set;
    }
    public bool IsMaxDist
    {
        set
        {
            m_isMaxDist = value;
        }
    }

    // Character Joint Data
    [SerializeField]
    private bool m_HasX, m_HasY, m_HasZ; // choose axes
    private Vector3 m_swingAxis; // joint swing axis
    [SerializeField]
    private float m_LowTwistLim = -100.0f;  // lower joint limit - primary axis
    [SerializeField]                   
    private float m_HighTwistLim = 100.0f;  // upper joint limit - primary axis                 
    [SerializeField]
    private float m_swing1Lim = 20.0f;
    [SerializeField]
    private float m_swing2Lim = 40.0f;

    //

    void Awake () {


        m_swingAxis = new Vector3(m_HasX ? 1 : 0, m_HasY ? 1 : 0, m_HasZ ? 1 : 0); // init vector based on bool values
        m_ropeRenderer = m_Emitter.AddComponent<LineRenderer>();
        m_ropeRenderer.material = m_RopeMaterial;
        m_ropeRenderer.SetWidth(m_ropeWidth, m_ropeWidth);
        m_ropeRenderer.enabled = false;
       // JointCache();


    }
    /* //FURTHER ITTERATION NEEDED
    private void JointCache()
    {
        for (int i = 0; i < m_PrecachedSegments; i++)
        {
            GameObject tempJoint = new GameObject("Joint " + i);
            SetupJoint(ref tempJoint);
            tempJoint.SetActive(false);
            m_RopeJoints.Add(tempJoint);

            tempJoint = null;
        }
    }
    */
    private void ComputeRope()
    {
        m_segments = (int)(Vector3.Distance(m_Emitter.transform.position, m_Target.transform.position) * m_segmentRate);
        m_ropeRenderer.SetVertexCount(m_segments);
        Vector3 heading = m_Target.transform.position - m_Emitter.transform.position;
        m_segmentSeparator = heading / (m_segments - 1);
        m_RopeSegments = new Vector3[m_segments]; //each time populate the array with nulls
        m_RopeJoints = new GameObject[m_segments];
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

    /// <summary>
    /// Character Joint setup function
    /// </summary>
    /// <param name="index"></param>
    private void SetupJointAT(int index)
    {
        m_RopeJoints[index] = new GameObject("Joint " + index);
        m_RopeJoints[index].transform.parent = m_Emitter.transform;
        m_RopeJoints[index].transform.position = m_RopeSegments[index];
        Rigidbody rb = m_RopeJoints[index].AddComponent<Rigidbody>();
        CharacterJoint cJoint = m_RopeJoints[index].AddComponent<CharacterJoint>();
        rb.useGravity = false;
        rb.drag = m_RopeSDrag;
        rb.mass = m_RopeSMass;
        SphereCollider sCol = m_RopeJoints[index].AddComponent<SphereCollider>();
        sCol.radius = m_RopeSColliderRadius;
        cJoint.swingAxis = m_swingAxis;
        SoftJointLimit limiter = cJoint.lowTwistLimit;
        limiter.limit = m_LowTwistLim;
        cJoint.lowTwistLimit = limiter;
        limiter = cJoint.highTwistLimit;
        limiter.limit = m_HighTwistLim;
        cJoint.highTwistLimit = limiter;
        limiter = cJoint.swing1Limit;
        limiter.limit = m_swing1Lim;
        cJoint.swing1Limit = limiter;
        limiter = cJoint.swing2Limit;
        limiter.limit = m_swing2Lim;
        cJoint.swing2Limit = limiter;
     //   cJoint.breakForce = m_RopeBreakForce;
       // cJoint.breakTorque = m_RopeBreakTorque;

        if (index == 1)
        {
            cJoint.connectedBody = m_Emitter.GetComponent<Rigidbody>();
        }
        else
        {
            cJoint.connectedBody = m_RopeJoints[index - 1].GetComponent<Rigidbody>();
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

