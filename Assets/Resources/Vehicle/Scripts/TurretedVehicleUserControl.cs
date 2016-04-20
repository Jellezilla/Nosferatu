using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleController), typeof(VehicleTurret))]
public class TurretedVehicleUserControl : MonoBehaviour {

    private VehicleController m_controller;
    private VehicleTurret m_turret;
    private VehicleResources m_resources;
    private float m_hAxis;
    private float m_vAxis;
    private float m_brakeAxis;
    private float m_fireAxis;

	// Use this for initialization
	void Awake ()
    {
        m_controller = GetComponent<VehicleController>();
        m_turret = GetComponent<VehicleTurret>();
        m_resources = GetComponent<VehicleResources>();
	}

    // Update is called once per frame

    private void GetInput()
    {
        m_hAxis = Input.GetAxis("Horizontal");
        m_vAxis = Input.GetAxis("Vertical");
        m_brakeAxis = Input.GetAxis("Jump");
        m_fireAxis = Input.GetAxis("Fire1");
    }

	void Update ()
    {

        GetInput();

        ///TESTING PAUSE STUFF
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

        }
        ///


        if (m_fireAxis == 1)
        {
            if (m_turret.isRetracted)
            {
                m_turret.Fire();
            }

        }
        if (m_fireAxis == 0 && Time.timeScale > 0)
        {
            if (!m_turret.isRetracted)
            {
                m_turret.Retract();
            }

        }

        m_turret.Aim();

    }

    void FixedUpdate()
    {
        m_controller.Motion(m_vAxis, m_hAxis, m_vAxis, m_brakeAxis);
    }
}
