using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour
{

    private Animator anim;
    [SerializeField]
    private GameObject m_rig;
    private bool m_isDead;
    [SerializeField]
    private GameObject m_BloodSplatterPrefab;
    [SerializeField]
    private GameObject m_SoulPrefab;
    private Rigidbody m_Hrb;
    private Collider[] m_HColls;
    [SerializeField]
    private Rigidbody[] m_rigRbs;
    private Vector3[] m_rigRbsPos;
    private Quaternion[] m_rigRbsRot;
    [SerializeField]
    private Collider[] m_rigCols;
    private bool m_init;
    private ParticleSystem m_BloodSplatPS;
    private GameObject m_Soul;
    private ParticleSystem[] m_SoulPSs;
    // Use this for initialization
    void Awake()
    {
        Init();

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
        if (!m_init)
        {
            anim = GetComponent<Animator>();
            m_Hrb = GetComponent<Rigidbody>();
            m_HColls = GetComponents<Collider>();
           // m_rigRbs = m_rig.GetComponentsInChildren<Rigidbody>();
            m_rigRbsPos = new Vector3[m_rigRbs.Length];
            m_rigRbsRot = new Quaternion[m_rigRbs.Length];
            for (int i = 0; i < m_rigRbs.Length; i++)
            {
                m_rigRbsPos[i] = m_rigRbs[i].transform.localPosition;
                m_rigRbsRot[i] = m_rigRbs[i].transform.localRotation;
                m_rigCols[i].enabled = false;
                m_rigRbs[i].useGravity = false;
            }
            float offset = m_HColls[1].bounds.extents.y;
            m_BloodSplatPS = (Instantiate(m_BloodSplatterPrefab)).GetComponentInChildren<ParticleSystem>();
            m_BloodSplatPS.transform.parent.SetParent(transform);
            m_BloodSplatPS.transform.parent.localPosition = new Vector3(0,offset,0);
            m_Soul = Instantiate(m_SoulPrefab);
            m_Soul.transform.SetParent(transform);
            m_Soul.transform.localPosition = new Vector3(0,offset,0);
            m_SoulPSs = m_Soul.GetComponentsInChildren<ParticleSystem>();
            m_init = true;
        }

    }

    /// <summary>
    /// Human reset method
    /// </summary>
    public void Reset()
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
                m_rigRbs[i].useGravity = false;
                m_rigRbs[i].velocity = Vector3.zero;
                m_rigRbs[i].angularVelocity = Vector3.zero;
            }
            for (int i = 0; i < m_SoulPSs.Length; i++)
            {
                m_SoulPSs[i].Stop();
            }


        }


    void Death(GameObject hiter)
    {
        Rigidbody hitterRB = hiter.GetComponent<Rigidbody>();
        Vector3 hitforce = hitterRB.velocity.normalized * hitterRB.velocity.magnitude;
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
                m_rigRbs[i].useGravity = true;
                m_rigRbs[i].AddForce(hitforce, ForceMode.Impulse);
            }
            m_BloodSplatPS.Play();
            for (int i = 0; i < m_SoulPSs.Length; i++)
            {
                m_SoulPSs[i].Play();
            }
        }

    }

    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {

        if (col.tag == Tags.player)
        {
            Death(col.gameObject);
        }
    }


}
