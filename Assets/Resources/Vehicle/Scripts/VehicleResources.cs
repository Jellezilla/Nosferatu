using UnityEngine;
using System.Collections;

/// <summary>
/// Vehicle Resource handler class
/// </summary>
public class VehicleResources : MonoBehaviour
{
    public GameObject UICanvasRef;
    private UIManager _uiManagerRef;

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
    private WaitForSeconds m_fuelWaitTick;

    private float m_CarBlood;
    private float m_CarSouls;

    private int m_playerMaxDistance;
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
    [SerializeField]
    private float m_FuelReplenishTime;
    // Use this for initialization
    void Awake()
    {
        m_fuelWaitTick = new WaitForSeconds(Time.fixedDeltaTime);
        _uiManagerRef = UICanvasRef.GetComponent<UIManager>();
        m_CarBlood = m_MaxCarBlood;
        StartCoroutine(FuelReplenishCheck());
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
            //Adding UI soul effect
            _uiManagerRef.AddSoulEffect(new Vector2(other.transform.position.x, other.transform.position.z));
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
                //Adding UI blood effect
                if (!_uiManagerRef.IsBloodFading())
                    _uiManagerRef.AddBloodEffect();
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

    IEnumerator FuelReplenishCheck()
    {
        float counter = m_FuelReplenishTime;
        while (!DeadInTheWater && counter > 0)
        {
            yield return m_fuelWaitTick;
            if (OutOfFuel)
            {
                counter -= Time.deltaTime;
            }

        }

        DeadInTheWater = true;

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
