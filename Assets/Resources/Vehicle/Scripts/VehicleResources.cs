using UnityEngine;
using System.Collections;

/// <summary>
/// Vehicle Resource handler class
/// </summary>
public class VehicleResources : MonoBehaviour
{

    [SerializeField]
    private float m_MaxCarSouls;
    [SerializeField]
    private float m_MaxCarBlood;
    [SerializeField]
    private float m_BloodConsumption;
    [SerializeField]
    private float m_BloodPerHuman;
    [SerializeField]
    private float m_SoulsPerHuman;
    [SerializeField]
    private GameObject m_explosion;
    private float m_CarBlood;
    private float m_CarSouls;
    private bool m_dead;
    private int m_playerMaxDistance;
    private bool m_deadIntheWater;
    private Rigidbody m_rb;
    public bool DeadInTheWater
    {
        private set;
        get;
    }
    public bool OutOfFuel
    {
        private set;
        get;
    }
    public bool InLava
    {
        private set;
        get;
    }
    [SerializeField]
    private float m_MovementThreshold;
    // Use this for initialization
    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_CarBlood = m_MaxCarBlood;
    }

    // Update is called once per frame
    void Update()
    {
        BloodConsumption();
        FuelReplenishCheck();
        CarDeath();
    }


    /// <summary>
    /// death effects function
    /// </summary>
    private void CarDeath()
    {
        if (m_deadIntheWater && !m_dead)
        {
            m_dead = true;
            Destroy(gameObject, 1.0f);
            Instantiate(m_explosion, transform.position,Quaternion.identity);
        }
    }
    void OnDestroy()
    {
        DeadInTheWater = true;
    }

    public void ConsumeSouls(float value)
    {
        m_CarSouls -= value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.lava)
        {
            m_CarBlood = 0;
            InLava = true;
        }

        if (other.tag == Tags.human)
        {
           
            if (m_CarSouls < m_MaxCarSouls)
            {
                m_CarSouls += m_SoulsPerHuman;

                if (m_CarSouls > m_MaxCarSouls)
                {
                    m_CarSouls = m_MaxCarSouls;
                }
            }

            if (m_CarBlood < m_MaxCarBlood)
            {
                m_CarBlood += m_BloodPerHuman;

                if (m_CarBlood > m_MaxCarBlood)
                {
                    m_CarBlood = m_MaxCarBlood;
                }
            }
        }
    }

    /// <summary>
    /// Blood Consumption Mechanic
    /// </summary>
    private void BloodConsumption()
    {
        if (m_CarBlood > 0)
        {

            m_CarBlood -= m_BloodConsumption * Time.deltaTime;
            OutOfFuel = false;

            if (m_CarBlood < 0)
            {
                m_CarBlood = 0;
            }
        }
        else
        {
            OutOfFuel = true;
        }
    }

    void FuelReplenishCheck()
    {

        if (OutOfFuel && m_rb.velocity.magnitude < m_MovementThreshold)
        {
            m_deadIntheWater = true;
        }
        else if(InLava)
        {
            m_deadIntheWater = true;
        }


    }
    #region Getters
    public float CarSouls
    {
        get
        {
            return m_CarSouls;
        }
    }

    public float CarBlood
    {
        get
        {
            return m_CarBlood;
        }
    }

    public float MaxCarBlood
    {
        get
        {
            return m_MaxCarBlood;
        }
    }

    public float MaxCarSouls
    {
        get
        {
            return m_MaxCarSouls;
        }
    }
    #endregion
}
