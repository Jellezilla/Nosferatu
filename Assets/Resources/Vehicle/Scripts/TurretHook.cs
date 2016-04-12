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
    private SpringJoint m_hinge;
    private HingeJoint m_oHinge;
    private Rigidbody m_launchPointRb;
    private Collider m_col;
    private float m_currentHookedDist;
    private float m_chainSpringForce;
    private float m_chainSpringDampning;

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


    /*
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

                   
                }
            }
        }
    }

    void OnTriggerExit()
    {
       
        m_currentHookedDist = 0;
    }
    */

    public void Launch(Rigidbody shooterRB,float springForce,float springDamper)
    {
        m_hooked = true;
        //m_currentHookedDist = Vector3.Distance(m_spawnPosition, transform.position);
        //add hinge to hook
        m_hinge = gameObject.AddComponent<SpringJoint>();
        m_hinge.axis = Vector3.up;
        m_hinge.anchor = Vector3.zero;
        m_hinge.spring = springForce;
        m_hinge.damper = springDamper;
        m_hinge.connectedBody = shooterRB;
    }

    public void Detach()
    {
        Destroy(GetComponent<SpringJoint>(),0.0f);
        m_hooked = false;
        m_hinge = null;
        gameObject.SetActive(false);
    }
    /*
    void ReelIn()
    {
        if (m_DragMode && !m_IsReset)
        {
            m_returnHeading = m_spawnPosition - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, m_spawnPosition, Time.fixedDeltaTime * m_ReturnForce);
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
    */

    /*
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
        else if (m_hooked)
        {
           
        }
    }
   */
    void Update()
    {
        
       // HookAtMaxLength();
    }
    void FixedUpdate()
    {
      //  ReelIn();
    }
}
