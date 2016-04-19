using UnityEngine;
using System.Collections;

/// <summary>Grappling hook class, handles grapling hook logic</summary>
public class TurretHook : MonoBehaviour
{

    private Rigidbody m_rb;
    private Vector3 m_spawnPosition;
    private bool m_hooked;
    private SpringJoint m_hinge;
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
}
