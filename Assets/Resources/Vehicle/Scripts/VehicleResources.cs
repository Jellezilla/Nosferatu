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

    private float m_CarBlood;
    private float m_CarSouls;

    public bool OutOfFuel
    {
        private set;
        get;
    }
    // Use this for initialization
    void Awake()
    {
        m_CarBlood = m_MaxCarBlood;
    }

    // Update is called once per frame
    void Update()
    {
        BloodConsumption();
    }

    public void OnTriggerEnter(Collider other)
    {
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
    #endregion
}
