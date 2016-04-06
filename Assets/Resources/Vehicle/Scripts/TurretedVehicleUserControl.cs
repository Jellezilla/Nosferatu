using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleController), typeof(VehicleTurret))]
public class TurretedVehicleUserControl : MonoBehaviour {

    private VehicleController m_controller;
    private VehicleTurret m_turret;
    private float m_hAxis;
    private float m_vAxis;
    private float m_brakeAxis;
    private float m_fireAxis;
    private float m_retractAxis;
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
        m_fireAxis = Input.GetAxis("Fire1");
        m_retractAxis = Input.GetAxis("Fire2");
        Debug.Log(m_fireAxis);

        if (m_fireAxis == 1)
        {
            m_turret.Fire();
        }

        if(m_retractAxis == 1)
        {
            m_turret.Retract();
        }

    }

    void FixedUpdate()
    {
        m_controller.Motion(m_vAxis, m_hAxis, m_vAxis, m_brakeAxis);
        m_turret.Aim();
    }
}
