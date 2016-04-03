using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleController), typeof(VehicleTurret))]
public class TurretedVehicleUserControl : MonoBehaviour {

    private VehicleController m_controller;
    private VehicleTurret m_turret;
    private float m_hAxis;
    private float m_vAxis;
    private float m_brakeAxis;
	// Use this for initialization
	void Awake ()
    {
        m_controller = GetComponent<VehicleController>();
        m_turret = GetComponent<VehicleTurret>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_hAxis = Input.GetAxis("Horizontal");
        m_vAxis = Input.GetAxis("Vertical");
        m_brakeAxis = Input.GetAxis("Jump");
	
	}

    void FixedUpdate()
    {
        m_controller.Motion(m_vAxis, m_hAxis, m_vAxis, m_brakeAxis);
        m_turret.Aim();

    }
}
