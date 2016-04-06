using UnityEngine;
using System.Collections;

/// <summary>Grappling hook class, handles grapling hook logic</summary>
public class TurretHook : MonoBehaviour
{
    private float m_launchForce;
    private float m_returnForce;
    private float m_returnDistance;
    private Rigidbody m_rb;
    private Vector3 m_spawnPosition;
    private float m_MaxChainLength;
    private bool m_hooked;
    private bool m_returnMode;
    private Vector3 m_returnVector;
    private float m_spawnDistance;

    public bool m_MaxLength
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
       
       
        //grab original Position
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != Tags.player)
        {
            if (collision.gameObject.tag == Tags.human || collision.gameObject.tag == Tags.cow)
            {

                //HIT SUCKABLE Object
            }
        }
    }

    public void Launch(Vector3 heading, float maxChainLength, float retractDistance, float launchForce, float returnForce)
    {

        m_MaxChainLength = maxChainLength;
        m_returnDistance = retractDistance;
        m_launchForce = launchForce;
        m_returnForce = returnForce;
        m_rb.isKinematic = false;
        m_rb.AddForce(heading * m_launchForce, ForceMode.Force);
    }


    void MaxLength()
    {
        if (m_spawnDistance >= m_MaxChainLength)
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_rb.isKinematic = true;
            m_MaxLength = true;
        }
    }
   
    void Update()
    {
        m_spawnDistance = Vector3.Distance(m_spawnPosition, transform.position);
        MaxLength();
    }
    void FixedUpdate()
    {
        
    }
}
