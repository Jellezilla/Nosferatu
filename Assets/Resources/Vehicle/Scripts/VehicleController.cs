using UnityEngine;
using System.Collections;

internal enum TCS
{
    FrontWheel,
    RearWheel,
    FourWheel
}

internal enum Steering
{
    FrontWheel,
    RearWheel
}

internal enum Braking
{
    FontWheel,
    RearWheel,
    FourWheel
}

 internal enum SpeedUnits
{
    KPH,
    MPH
}

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour {

    private Rigidbody m_carRB;
    [SerializeField]
    private TCS m_TractionControl;
    [SerializeField]
    private Steering m_SteeringControl;
    [SerializeField]
    private Braking m_BrakingControl;
    [SerializeField]
    private SpeedUnits m_SpeedUnits;
    [SerializeField]
    private WheelCollider[] m_WheelColliders;
    [SerializeField]
    private GameObject[] m_WheelMeshes;
    [SerializeField]
    private Vector3 m_CenterOfMass;
    [SerializeField]
    private float m_MaxSteerAngle;
    [SerializeField]
    private float m_SteerSpeedThreshold;
    [SerializeField]
    private float m_SteerAngleThreshold;
    [Range(0, 1)]
    [SerializeField]
    private float m_SteerAsist;
    [Range(0, 1)]
    [SerializeField]
    private float m_TractionControlAsist;
    [SerializeField]
    private float m_WheelTorque;
    [SerializeField]
    private float m_ReverseTorque;
    private float m_MaxHandbrakeTorque;
    [SerializeField]
    private float m_Downforce;
    [SerializeField]
    private float m_Topspeed = 0;
    [SerializeField]
    private int m_nrOfGears = 0;
    [SerializeField]
    private float m_SlipLimit;
    [SerializeField]
    private float m_BrakeTorque;

    private Quaternion[] m_WheelMeshLocalRotations;
    private Vector3 m_Prevpos, m_Pos;
    private float m_SteerAngle;
    private int m_GearNum;
    private float m_GearFactor;
    private float m_OldRotation;
    private float m_CurrentTorque;
    private Rigidbody m_Rigidbody;
    private const float k_ReversingThreshold = 0.01f;
    private float m_mphFactor = 2.23693629f;
    private float m_kphFactor = 3.6f;

    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }
    public float CurrentSteerAngle { get { return m_SteerAngle; } }
    public float CurrentSpeed { get { return m_Rigidbody.velocity.magnitude * m_mphFactor; } } // Unity works in meters, 1 unit = 2.23693629f MPH
    public float MaxSpeed { get { return m_Topspeed; } }
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }
    public bool Grounded { get; private set; }

    // Use this for initialization
    void Start() {

        Init();

    }


    void FixedUpdate()
    {
        IsGrounded();
    }
    /// <summary>
    /// Init the controller
    /// </summary>
    private void Init()
    {
        m_MaxHandbrakeTorque = float.MaxValue;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.centerOfMass = m_CenterOfMass;
        m_CurrentTorque = m_WheelTorque - (m_TractionControlAsist * m_WheelTorque);
        m_WheelMeshLocalRotations = new Quaternion[m_WheelMeshes.Length];

        for (int i = 0; i < m_WheelMeshes.Length; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }

    }

    /// <summary>
    /// Gear changing, based on max speed & number of gears
    /// </summary>
    private void Gears()
    {
        float speedRatio = Mathf.Abs(CurrentSpeed / m_Topspeed);
        float upGearLimit = ((1 / m_nrOfGears) * (m_GearNum + 1));
        float downGearLimit = ((1 / m_nrOfGears) * m_GearNum);

        if (m_GearNum > 0 && speedRatio < downGearLimit)
        {
            m_GearNum--;
        }
        else if(m_GearNum > upGearLimit && (m_GearNum < (m_nrOfGears - 1)))
        {
            m_GearNum++;
        }
    }

    /// <summary>
    /// Used to cap speed based on use unit.
    /// </summary>
    private void SpeedLimiter()
    {
        float cSpeed = m_Rigidbody.velocity.magnitude;
        switch (m_SpeedUnits)
        {
            case SpeedUnits.MPH:
                {
                    cSpeed *= m_mphFactor;
                    if (cSpeed > m_Topspeed)
                    {
                        m_Rigidbody.velocity = (m_Topspeed / m_mphFactor) * m_Rigidbody.velocity.normalized;
                    }
                    break;
                }
            case SpeedUnits.KPH:
                {
                    cSpeed *= m_kphFactor;
                    if (cSpeed > m_Topspeed)
                    {
                        m_Rigidbody.velocity = (m_Topspeed / m_kphFactor) * m_Rigidbody.velocity.normalized;
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// Handle wheel drive forces.
    /// </summary>
    /// <param name="acceleration">current acceleration force</param>
    /// <param name="brake">current foot brake force</param>
    private void DrivingForcesApplication(float acceleration, float brake)
    {
        float thrustTorque;

        switch (m_TractionControl)
        { 
            case TCS.FrontWheel:
                {
                    thrustTorque = acceleration * (m_CurrentTorque / 2f);
                    m_WheelColliders[0].motorTorque = thrustTorque;
                    m_WheelColliders[1].motorTorque = thrustTorque;
                    break;
                }
            case TCS.RearWheel:
                {
                    thrustTorque = acceleration * (m_CurrentTorque / 2f);
                    m_WheelColliders[m_WheelColliders.Length-2].motorTorque = thrustTorque;
                    m_WheelColliders[m_WheelColliders.Length-1].motorTorque = thrustTorque;
                    break;
                }
            case TCS.FourWheel:
                {
                    thrustTorque = acceleration * (m_CurrentTorque / m_WheelColliders.Length);
                    for (int i = 0; i < m_WheelColliders.Length; i++)
                    {
                        m_WheelColliders[i].motorTorque = thrustTorque;
                    }
                    break;
                }
        }



        switch (m_TractionControl)
        {
            case TCS.FrontWheel:
                {

                    if (CurrentSpeed > m_SteerSpeedThreshold && Vector3.Angle(transform.forward, m_Rigidbody.velocity) > m_SteerAngleThreshold)
                    {
                        m_WheelColliders[0].brakeTorque = m_BrakeTorque * brake;
                        m_WheelColliders[1].brakeTorque = m_BrakeTorque * brake;
                    }
                    //add reverse & remove brake
                    else if (brake > 0)
                    {
                        m_WheelColliders[0].motorTorque = -m_ReverseTorque * brake;
                        m_WheelColliders[1].motorTorque = -m_ReverseTorque * brake;
                        for (int i = 0; i < m_WheelColliders.Length; i++)
                        {
                            m_WheelColliders[i].brakeTorque = 0;
                        }

                    }
                    //remove break to accelerate again
                    else if (acceleration > 0)
                    {
                        for (int i = 0; i < m_WheelColliders.Length; i++)
                        {
                            m_WheelColliders[i].brakeTorque = 0;
                        }
                    }
                    break;
                }
            case TCS.RearWheel:
                {

                    if (CurrentSpeed > m_SteerSpeedThreshold && Vector3.Angle(transform.forward, m_Rigidbody.velocity) > m_SteerAngleThreshold)
                    {
                        m_WheelColliders[m_WheelColliders.Length - 2].brakeTorque = m_BrakeTorque * brake;
                        m_WheelColliders[m_WheelColliders.Length - 1].brakeTorque = m_BrakeTorque * brake;
                    }
                    //add reverse & remove brake
                    else if (brake > 0)
                    {

                        m_WheelColliders[m_WheelColliders.Length - 2].motorTorque = -m_ReverseTorque * brake;
                        m_WheelColliders[m_WheelColliders.Length - 1].motorTorque = -m_ReverseTorque * brake;
                        for (int i = 0; i < m_WheelColliders.Length; i++)
                        {
                            m_WheelColliders[i].brakeTorque = 0;
                        }

                    }
                    //remove break to accelerate again
                    else if (acceleration > 0)
                    {
                        for (int i = 0; i < m_WheelColliders.Length; i++)
                        {
                            m_WheelColliders[i].brakeTorque = 0;
                        }
                    }

                    break;
                }
            case TCS.FourWheel:
                {
                    for (int i = 0; i < m_WheelColliders.Length; i++)
                    {
                        //if speed & angle between (heading,velocity) 
                        //exceed certain treshholds start braking  
                        if (CurrentSpeed > m_SteerSpeedThreshold && Vector3.Angle(transform.forward, m_Rigidbody.velocity) > m_SteerAngleThreshold)
                        {
                            m_WheelColliders[i].brakeTorque = m_BrakeTorque * brake;
                        }
                        //add reverse & remove brake
                        else if (brake > 0)
                        {
                            m_WheelColliders[i].brakeTorque = 0;
                            m_WheelColliders[i].motorTorque = -m_ReverseTorque * brake;
                        }
                        //remove break to accelerate again
                        else if (acceleration > 0)
                        {
                            m_WheelColliders[i].brakeTorque = 0;
                        }
                    }


                        break;
                }
        }






    }

    /// <summary>
    /// Check if the car is grounded
    /// </summary>
    private void IsGrounded()
    {

        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            WheelHit hit;
            if (!m_WheelColliders[i].GetGroundHit(out hit))
            {
                //if wheels off the ground then do not align velocity
                Grounded = false;
                return;
            }
            else
            {
                Grounded = true;
            }
        }
    }

    /// <summary>
    /// Controls steering behaviour for wheels 
    /// </summary>
    private void SteeringAsist()
    {
        if (!Grounded)
        {
            return;
        }

        //gimbal lock fix
        if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnAdj = (transform.eulerAngles.y - m_OldRotation) * m_SteerAsist;
            Quaternion velocityRot = Quaternion.AngleAxis(turnAdj, transform.up);
            m_Rigidbody.velocity = velocityRot * m_Rigidbody.velocity;
        }

        m_OldRotation = transform.eulerAngles.y;
    }

    /// <summary>
    /// Add extra force, to improve grip in relation to speed
    /// </summary>
    private void AddExtraGForce()
    {
        m_Rigidbody.AddForce(-transform.up * m_Downforce * m_Rigidbody.velocity.magnitude);
    }

    /// <summary>
    /// Adjust torque by force drift
    /// </summary>
    /// <param name="forwardSlip"></param>
    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
        {
            m_CurrentTorque -= 10 * m_TractionControlAsist;
        }
        else
        {
            m_CurrentTorque += 10 * m_TractionControlAsist;
            if (m_CurrentTorque > m_WheelTorque)
            {
                m_CurrentTorque = m_WheelTorque;
            }
        }
    }

    /// <summary>
    /// Crude traction control, reduces the power to wheel if the car is drifting too much
    /// </summary>
    private void TractionControl()
    {
        WheelHit hit;
        switch (m_TractionControl)
        {
            case TCS.FrontWheel:
                {
                    m_WheelColliders[0].GetGroundHit(out hit);
                    AdjustTorque(hit.forwardSlip);
                    m_WheelColliders[1].GetGroundHit(out hit);
                    AdjustTorque(hit.forwardSlip);
                    break;
                }
            case TCS.RearWheel:
                {
                    m_WheelColliders[m_WheelColliders.Length - 2].GetGroundHit(out hit);
                    AdjustTorque(hit.forwardSlip);
                    m_WheelColliders[m_WheelColliders.Length - 1].GetGroundHit(out hit);
                    AdjustTorque(hit.forwardSlip);
                    break;
                }
            case TCS.FourWheel:
                {
                    for (int i = 0; i < m_WheelColliders.Length; i++)
                    {
                        m_WheelColliders[i].GetGroundHit(out hit);
                        AdjustTorque(hit.forwardSlip);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// Vehicle controller movement function, called continously to move the controller, asumes front wheels are for steering
    /// </summary>
    /// <param name="acceleration"></param>
    /// <param name="steering"></param>
    /// <param name="brake"></param>
    /// <param name="handBrake"></param>
    public void Motion(float acceleration, float steering, float brake, float handBrake)
    {
        for (int i = 0; i < m_WheelColliders.Length; i++)
        {
            Quaternion rotation;
            Vector3 position;

            m_WheelColliders[i].GetWorldPose(out position, out rotation);

            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = rotation;
        }

        //clamping input values
        steering = Mathf.Clamp(steering, -1, 1);
        acceleration = Mathf.Clamp(acceleration, 0, 1);
        brake = -1*Mathf.Clamp(brake, -1, 0);
        handBrake = Mathf.Clamp(handBrake, 0, 1);

        //passing inputs
        AccelInput = acceleration;
        BrakeInput = brake;

        //set the steering
        m_SteerAngle = steering * m_MaxSteerAngle;
        switch (m_SteeringControl)
        {
            case Steering.FrontWheel:
                {
                    m_WheelColliders[0].steerAngle = m_SteerAngle;
                    m_WheelColliders[1].steerAngle = m_SteerAngle;
                    break;
                }
            case Steering.RearWheel:
                {
                    m_WheelColliders[m_WheelColliders.Length-2].steerAngle = m_SteerAngle;
                    m_WheelColliders[m_WheelColliders.Length-1].steerAngle = m_SteerAngle;
                    break;
                }
        }

        DrivingForcesApplication(acceleration, brake);
        // set the braking
        if (handBrake > 0)
        {
            var brakeTorque = handBrake * m_MaxHandbrakeTorque;
            switch (m_BrakingControl)
            {
                case Braking.FontWheel:
                    {
                        m_WheelColliders[0].brakeTorque = brakeTorque;
                        m_WheelColliders[1].brakeTorque = brakeTorque;
                        break;
                    }
                case Braking.RearWheel:
                    {
                        m_WheelColliders[m_WheelColliders.Length - 2].brakeTorque = brakeTorque;
                        m_WheelColliders[m_WheelColliders.Length - 1].brakeTorque = brakeTorque;
                        break;
                    }
                case Braking.FourWheel:
                    {
                        for (int i = 0; i < m_WheelColliders.Length; i++)
                        {
                            m_WheelColliders[i].brakeTorque = brakeTorque;
                        }
                        break;
                    }
            }

        }

        //call controller functions
        SteeringAsist();

        SpeedLimiter();
        Gears();
        AddExtraGForce();
        TractionControl();



    }
}
