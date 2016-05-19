using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VehicleController), typeof(VehicleTurret))]
public class TurretedVehicleUserControl : MonoBehaviour {

    private VehicleController m_controller;
    private VehicleTurret m_turret;
    private VehicleResources m_resources;
    private VehicleNitro m_nitro;
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
        m_nitro = GetComponent<VehicleNitro>();
	}

    // Update is called once per frame

    private void GetInput()
    {
        if (m_resources.OutOfFuel)
        {
            m_vAxis = 0;
        }
        else
        {
            
            m_vAxis = Input.GetAxis("Vertical");
        }
        m_hAxis = Input.GetAxis("Horizontal");
        m_brakeAxis = Input.GetAxis("Jump");
        m_fireAxis = Input.GetAxis("Fire1");

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_controller.LightsToggle();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            //EventController.Instance.TriggerEvent(UIEvents.Rampage);
            m_nitro.UseNitro();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //EventController.Instance.TriggerEvent(UIEvents.Rampage);
            m_nitro.ReleaseNitro();
        }
    }

	void Update ()
    {

        GetInput();
        TurretLogic();





    }

    private void TurretLogic()
    {
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
