using UnityEngine;
using System.Collections;

public class VehicleNitro : MonoBehaviour {

    [SerializeField]
    private float m_SoulConsumptionValue;
    [SerializeField]
    private float m_SoulConsumptionRate;
    [SerializeField]
    private float m_boostForce;
    [SerializeField]
    private GameObject m_Thrusters;
    private ParticleSystem[] m_ThrusterPSs;
    private VehicleResources m_carRes;
    private Rigidbody m_playerRB;
    public bool UsingNitro
    {
        private set;
        get;
    }
	// Use this for initialization
	void Start ()
    {
        m_ThrusterPSs = m_Thrusters.GetComponentsInChildren<ParticleSystem>();
        m_playerRB = GetComponent<Rigidbody>();
        m_carRes = GetComponent<VehicleResources>();

	}


    public void UseNitro()
    {
        if (m_carRes.CarSouls > 0 && m_carRes.CarSouls > (m_SoulConsumptionValue * Time.deltaTime * m_SoulConsumptionRate))
        {
            m_playerRB.AddForce(transform.forward * m_boostForce);
            m_carRes.ConsumeSouls(m_SoulConsumptionValue * Time.deltaTime * m_SoulConsumptionRate);

            for (int i = 0; i < m_ThrusterPSs.Length; i++)
            {
                if (!m_ThrusterPSs[i].isPlaying)
                {
                    m_ThrusterPSs[i].Play();
                }

            }

            UsingNitro = true;

        }
        else
        {
            for (int i = 0; i < m_ThrusterPSs.Length; i++)
            {
                if (m_ThrusterPSs[i].isPlaying)
                {
                    m_ThrusterPSs[i].Stop();
                }
            }
        }
    }

    public void ReleaseNitro()
    {
        for (int i = 0; i < m_ThrusterPSs.Length; i++)
        {
            m_ThrusterPSs[i].Stop();
        }
        UsingNitro = false;
    }

	// Update is called once per frame
	void Update ()
    {

    }
}
