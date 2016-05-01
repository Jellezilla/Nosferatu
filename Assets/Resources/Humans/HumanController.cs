using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour
{

    private Animator anim;
    [SerializeField]
    private GameObject m_rig;
    private bool m_isDead;
    public GameObject bloodSpatterObject;
    private Rigidbody m_Hrb;
    private Collider[] m_HColls;
    private Rigidbody[] m_rigRbs;
    private Vector3[] m_rigRbsPos;
    private Quaternion[] m_rigRbsRot;
    private Collider[] m_rigCols;
    private bool m_init;

    // Use this for initialization
    void Awake()
    {
        Init();

    }

    void OnEnable()
    {
        //check if human initialized, if not then initialize
        if (!m_init)
        {
            Init();
        }
    }

    void OnDisable()
    {
        Reset();
    }

    /// <summary>
    ///  Initialize references and variables
    /// </summary>
	void Init()
    {

        anim = GetComponent<Animator>();
        m_Hrb = GetComponent<Rigidbody>();
        m_HColls = GetComponents<Collider>();
        m_rigRbs = m_rig.GetComponentsInChildren<Rigidbody>();
        m_rigRbsPos = new Vector3[m_rigRbs.Length];
        m_rigRbsRot = new Quaternion[m_rigRbs.Length];
        for (int i = 0; i < m_rigRbs.Length; i++)
        {
            m_rigRbsPos[i] = m_rigRbs[i].transform.localPosition;
            m_rigRbsRot[i] = m_rigRbs[i].transform.localRotation;
        } 
        m_rigCols = m_rig.GetComponentsInChildren<Collider>();
        m_init = true;
        Reset();
    }

    /// <summary>
    /// Human reset method
    /// </summary>
    void Reset()
    {
        if (m_isDead)
        {
            m_isDead = false;
            anim.enabled = true;
            m_Hrb.isKinematic = false;
            for (int i = 0; i < m_HColls.Length; i++)
            {
                m_HColls[i].enabled = true;
            }

            for (int i = 0; i < m_rigRbs.Length; i++)
            {
                m_rigCols[i].enabled = false;
                m_rigRbs[i].transform.localPosition = m_rigRbsPos[i];
                m_rigRbs[i].transform.localRotation = m_rigRbsRot[i];
                m_rigRbs[i].isKinematic = true;
                m_rigRbs[i].useGravity = false;

            }
        }

    }

    void Death()
    {
        if (!m_isDead)
        {
            m_isDead = true;
            anim.enabled = false;
            m_Hrb.isKinematic = true;

            for (int i = 0; i < m_HColls.Length; i++)
            {
                m_HColls[i].enabled = false;
            }

            for (int i = 0; i < m_rigRbs.Length; i++)
            {
                m_rigCols[i].enabled = true;
                m_rigRbs[i].isKinematic = false;
                m_rigRbs[i].useGravity = true;
            }

            (Instantiate(bloodSpatterObject, new Vector3(transform.position.x, transform.position.y , transform.position.z), transform.rotation) as GameObject).transform.parent = transform;

        }

    }

    void OnTriggerEnter(Collider col)
    {

        if (col.tag == Tags.player)
        {
            Death();
        }
    }


}
