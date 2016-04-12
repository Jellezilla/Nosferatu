using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(TurretedVehicleUserControl))]
public class VehicleTurret : MonoBehaviour {

    [SerializeField]
    private GameObject m_VehicleTurret;
    [SerializeField]
    private float m_spawnPointOffset;
    private Vector3 m_spawnPoint;
    private float m_OldRotation;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    private Vector3 m_prevMPos;
    private Vector3 m_curMPos;
    [SerializeField]
    private int m_RotRevCooldown;
    private WaitForFixedUpdate m_WaitStep;
    private bool m_Rotating;
    [SerializeField]
    private GameObject m_HookPrefab;
    [SerializeField]
    private int m_MaxChainLength;
    [SerializeField]
    private float m_RopeSpringDampning;
    [SerializeField]
    private float m_RopeSpringForce;
    [SerializeField]
    private float m_RetractPointDist;
    [SerializeField]
    private float m_LaunchForce;
    [Tooltip("Hook return factor")]
    [Range(10,60)]
    [SerializeField]
    private float m_ReturnFactor;
    private TurretHook m_Hook;
    private VehicleTurretRope m_Chain;
    private Rigidbody m_rb;
    private bool m_retracted;

    public bool isRetracted
    {
        get
        {
            return m_retracted;
        }
    }
	// Use this for initialization

    public Vector3 SpawnPoint
    {
        get
        {
            return m_spawnPoint;
        }
    }

	void Start () {

        m_retracted = true;
        m_Hook = ((GameObject)Instantiate(m_HookPrefab, m_spawnPoint, transform.rotation)).GetComponent<TurretHook>();
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), m_Hook.gameObject.GetComponent<Collider>());
        m_Hook.gameObject.SetActive(false);
        m_Chain = GetComponent<VehicleTurretRope>();
        m_prevMPos = Input.mousePosition;
        m_WaitStep = new WaitForFixedUpdate();
        m_rb = GetComponent<Rigidbody>();
        StartCoroutine(ReverseRotation());

    }

    /// <summary>
    /// Used to Calculate Spawn Point
    /// </summary>
    private void CalculateSpawnPoint()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Used to set the turret back to its original rotation if the player doesnt move the mouse
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReverseRotation()
    {
        float rotCounter=0;
        while (true)
        {
            if (m_prevMPos == Input.mousePosition)
            {
                rotCounter+=Time.fixedDeltaTime;
                if (rotCounter >= m_RotRevCooldown)
                {
                    m_Rotating = false;
                    if (m_VehicleTurret.transform.rotation == transform.rotation)
                    {
                        rotCounter = 0;
                    }
                }
            }
            else
            {
                rotCounter = 0;
                m_Rotating = true;
            }
            yield return m_WaitStep;
        }
    }

    /// <summary>
    /// Aim function
    /// </summary>
    public void Aim()
    {
        if (!m_Hook.hooked)
        {
            if (m_Rotating)
            {
                var pos = Camera.main.WorldToScreenPoint(m_VehicleTurret.transform.position);
                m_prevMPos = Input.mousePosition;
                var dir = m_prevMPos - pos;
                var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                m_VehicleTurret.transform.rotation = Quaternion.Lerp(m_VehicleTurret.transform.rotation, Quaternion.AngleAxis(angle, transform.up), Time.deltaTime * m_rotationSpeed);

            }
            else
            {
                m_VehicleTurret.transform.rotation = Quaternion.Lerp(m_VehicleTurret.transform.rotation, transform.rotation, Time.deltaTime * m_rotationSpeed);
            }
        } 



    }

    /// <summary>
    /// Used to fire the hook
    /// </summary>
    public void Fire()
    {
        RaycastHit hit;

        if (!m_Hook.gameObject.activeSelf)
        {
           
            if (Physics.Raycast(m_VehicleTurret.transform.position, m_VehicleTurret.transform.forward, out hit, m_MaxChainLength, -1)) // 0 is default layer
            {
                if (hit.collider.gameObject.tag != Tags.human)
                {
                    m_Hook.gameObject.SetActive(true);
                    m_Hook.gameObject.transform.rotation = m_VehicleTurret.transform.rotation;
                    m_Hook.transform.position = hit.point;
                    m_Hook.Launch(m_rb, m_RopeSpringForce, m_RopeSpringDampning);
                    m_Chain.CreateRope(m_Hook.gameObject);
                    m_retracted = false;
                }


            }
        }

        /*
        if (!m_Hook.gameObject.activeSelf)
        {
            m_Hook.gameObject.SetActive(true);
            m_Hook.transform.rotation = m_VehicleTurret.transform.rotation;
            m_Hook.transform.position = m_spawnPoint;
            m_Hook.Launch(m_VehicleTurret.transform.forward, m_MaxChainLength, m_RetractPointDist, m_LaunchForce, m_RopeSpringForce, m_RopeSpringDampning, m_ReturnFactor, m_rb);
            m_Chain.CreateRope(m_Hook.gameObject);
        }
        */

    }

    /// <summary>
    /// Used to retract the hook
    /// </summary>
    public void Retract()
    {
        m_Hook.Detach();
        m_Chain.DestroyRope();
        m_retracted = true;
    }

    /// <summary>
    /// Hook 
    /// </summary>
    void HookAndChainLogic()
    {

        if (!m_Hook.m_IsReset && m_Hook.gameObject.activeSelf)
        {
            if (!m_Hook.m_DragMode)
            {
                m_retracted = false;
            }

        }
        else if(!m_retracted)
        {
            Retract();
            m_Chain.DestroyRope();
            m_retracted = true;

        }

    }

    private void HookSpawnPoint()
    {

        m_spawnPoint = m_VehicleTurret.transform.position;
        m_spawnPoint.z += m_spawnPointOffset;

        if (m_Hook.gameObject.activeSelf)
        {
          //  m_Hook.spawnPosition = m_spawnPoint;
        }
    }

    void Update()
    {
        HookSpawnPoint();
       // HookAndChainLogic();

    }

    void FixedUpdate()
    {
        
    }
	
}
