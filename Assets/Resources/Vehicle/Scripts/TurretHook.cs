using UnityEngine;
using System.Collections;

/// <summary>Grappling hook class, handles grapling hook logic</summary>
public class TurretHook : MonoBehaviour
{
    private float m_launchForce;
    private float m_ReturnForce;
    private float m_returnDistance;
    private Rigidbody m_rb;
    private Vector3 m_spawnPosition;
    private float m_MaxChainLength;
    private bool m_hooked;
    private bool m_returnMode;
    private float m_spawnDistance;
    private Vector3 m_returnHeading;
    private HingeJoint m_hinge;
    private HingeJoint m_oHinge;
    private Rigidbody m_launchPointRb;
    private Collider m_col;

    public bool hooked
    {
        get
        {
            return m_hooked;
         }
    }

    public bool m_DragMode
    {
        get;
        private set;
    }

    public bool m_IsReset
    {
        get;
        private set; 
    }
    public Vector3 spawnPosition
    {
        set
        {
            m_spawnPosition = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_col = GetComponent<Collider>();
       
       
        //grab original Position
    }



    void OnTriggerEnter(Collider collision)
    {
        if (!m_DragMode)
        {
            if (collision.gameObject.tag != Tags.player)
            {
                if (collision.gameObject.tag == Tags.human || collision.gameObject.tag == Tags.cow)
                {


                }
                else
                {

                    m_rb.velocity = Vector3.zero;
                    m_rb.isKinematic = true;
                    m_col.enabled = false;
                    m_rb.transform.rotation = Quaternion.identity;

                    m_hooked = true;

                    //add hinge to hook
                    m_hinge = gameObject.AddComponent<HingeJoint>();
                    m_hinge.axis = Vector3.up;
                    m_hinge.anchor = Vector3.zero;
                    m_hinge.connectedBody = m_launchPointRb;
                }
            }
        }
    }

    void OnTriggerExit()
    {
        m_hinge = null;
    }

    public void Launch(Vector3 heading, float maxChainLength, float retractDistance, float launchForce, float returnForce, Rigidbody origin)
    {
        m_col.enabled = true;
        m_launchPointRb = origin;
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_MaxChainLength = maxChainLength;
        m_returnDistance = retractDistance;
        m_launchForce = launchForce;
        m_ReturnForce = returnForce;
        m_IsReset = false;
        m_rb.AddForce(heading * m_launchForce, ForceMode.Impulse);
    }

    public void Detach()
    {
        Destroy(GetComponent<HingeJoint>(),0.0f);
        m_rb.isKinematic = false;

        m_hooked = false;
        m_DragMode = true;
    }
    void ReelIn()
    {
        if (m_DragMode && !m_IsReset)
        {
            m_returnHeading = m_spawnPosition - transform.position;
            m_rb.transform.position = Vector3.MoveTowards(m_rb.transform.position, m_spawnPosition, Time.fixedDeltaTime * m_ReturnForce);
            if (m_spawnDistance < m_returnDistance)
            {
                m_rb.velocity = Vector3.zero;
                m_rb.angularVelocity = Vector3.zero;
                m_DragMode = false;
                m_IsReset = true;
                gameObject.SetActive(false);
            }
        }
    }

    void HookAtMaxLength()
    {
        m_spawnDistance = Vector3.Distance(m_spawnPosition, transform.position);
        if (m_spawnDistance >= m_MaxChainLength && !m_DragMode && !m_hooked)
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_DragMode = true;
          //  Vector3 heading = m_spawnPosition - transform.position;
           // m_rb.AddForce(heading * m_returnForce, ForceMode.Force);
           

        }
    }
   
    void Update()
    {
        
        HookAtMaxLength();
    }
    void FixedUpdate()
    {
        ReelIn();
    }
}
