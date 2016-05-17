using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleTurretRope))]
public class VehicleTurret : MonoBehaviour {

    [SerializeField]
    private GameObject m_VehicleTurret;
    private float m_OldRotation;
    private Vector3 m_prevMPos;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private int m_RotRevCooldown;
    private WaitForFixedUpdate m_turretWaitStep;
    private bool m_Rotating;
    [SerializeField]
    private GameObject m_HookPrefab;
    [SerializeField]
    private int m_MaxRopeLength;
    private float m_CurentRopeLength;
    [SerializeField]
    private float m_RopeSpringForce;
    [SerializeField]
    private float m_RopeSpringDamper;
    private GameObject m_Hook;
    private Rigidbody m_HookRB;
    private VehicleTurretRope m_Rope;
    private bool m_retracted;
    private HingeJoint m_turretChainJoint;
    private HingeJoint m_turretHookJoint;
    private JointSpring m_spring;
    public bool isRetracted
    {
        get
        {
            return m_retracted;
        }
    }

	void Start () {

        InitTurret();
        InitHook();

    }

    /// <summary>
    /// Init turret
    /// </summary>
    void InitTurret()
    {
        m_Rope = GetComponent<VehicleTurretRope>();
        m_turretWaitStep = new WaitForFixedUpdate();
        m_retracted = true;
        m_prevMPos = Input.mousePosition;
        StartCoroutine(ReverseRotation());
    }

    /// <summary>
    /// Init hook
    /// </summary>
    void InitHook()
    {
        m_Hook = Instantiate(m_HookPrefab);
        m_HookRB = m_Hook.GetComponent<Rigidbody>();
        m_Hook.gameObject.SetActive(false);
        m_spring = new JointSpring();
        m_spring.targetPosition = 0;
        m_spring.spring = m_RopeSpringForce;
        m_spring.damper = m_RopeSpringDamper;
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
            yield return m_turretWaitStep;
        }
    }

    /// <summary>
    /// Aim function
    /// </summary>
    public void Aim()
    {
        if (m_retracted)
        {
            if (m_Rotating)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(m_VehicleTurret.transform.position);
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
        if (!m_Hook.gameObject.activeSelf)
        {
            if (GameController.Instance.InteractionManager.SelectedObject != null)
            {
                GameObject target = GameController.Instance.InteractionManager.SelectedObject.gameObject;

                if (Vector3.Distance(transform.position, target.transform.position) <= m_MaxRopeLength)
                {

                    m_Hook.gameObject.SetActive(true);
                    m_Hook.gameObject.transform.rotation = m_VehicleTurret.transform.rotation;
                    Vector3 hitpos = target.transform.position;
                    hitpos.y = m_VehicleTurret.transform.position.y;
                    m_Hook.transform.position = hitpos;

                    //add hook hinge
                    m_turretHookJoint = m_Hook.gameObject.AddComponent<HingeJoint>();
                    // m_turretHookJoint.spring = m_spring;
                    // m_turretHookJoint.useSpring = true;
                    m_turretHookJoint.axis = Vector3.up;
                    m_turretHookJoint.anchor = Vector3.zero;

                    //add car hinge
                    m_turretChainJoint = gameObject.AddComponent<HingeJoint>();
                    m_turretChainJoint.spring = m_spring;
                    m_turretChainJoint.useSpring = true;
                    m_turretChainJoint.anchor = Vector3.zero;
                    m_turretChainJoint.axis = Vector3.up;
                    m_turretChainJoint.connectedBody = m_HookRB;
                    m_Rope.CreateRope(m_Hook.gameObject);
                    m_CurentRopeLength = Vector3.Distance(m_Hook.transform.position, gameObject.transform.position);
                    m_retracted = false;

                }
            }

        }

    }

    /// <summary>
    /// Used to retract the hook
    /// </summary>
    public void Retract()
    {
        Destroy(m_turretChainJoint, 0.0f);
        Destroy(m_turretHookJoint, 0.0f);
        m_Hook.gameObject.SetActive(false);
        m_Rope.DestroyRope();
        m_retracted = true;
    }

    /// <summary>
    /// Hook and chain distance clamping
    /// </summary>
    void HookAndChainLogic()
    {
        if (!m_retracted)
        {
            if (Vector3.Distance(transform.position, m_Hook.transform.position) > m_CurentRopeLength)
            {
                // Vector3 counterForceHeading = m_Hook.transform.position - transform.position;
                //  m_rb.AddForce(counterForceHeading * 1000);
            }
        }


    }


    void Update()
    {
        HookAndChainLogic();

    }
	
}
