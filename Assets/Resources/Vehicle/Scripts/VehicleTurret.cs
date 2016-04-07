﻿using UnityEngine;
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
    [SerializeField]
    private int m_RotRevCooldown;
    private WaitForSeconds m_WaitStep;
    private bool m_Rotating;
    [SerializeField]
    private GameObject m_HookPrefab;
    [SerializeField]
    private int m_MaxChainLength;
    [SerializeField]
    private float m_RetractPointDist;
    [SerializeField]
    private float m_LaunchForce;
    [SerializeField]
    private float m_ReturnForce;
    private TurretHook m_Hook;
    private VehicleTurretRope m_Chain;
    private Rigidbody m_rb;
    [SerializeField]
    private float m_angularRotationFactor;
	// Use this for initialization

    public Vector3 SpawnPoint
    {
        get
        {
            return m_spawnPoint;
        }
    }

	void Start () {

        m_Hook = ((GameObject)Instantiate(m_HookPrefab, m_spawnPoint, transform.rotation)).GetComponent<TurretHook>();
        m_Hook.gameObject.SetActive(false);
        m_Chain = GetComponent<VehicleTurretRope>();
        m_prevMPos = Input.mousePosition;
        m_WaitStep = new WaitForSeconds(1);
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
        int rotCounter=0;
        while (true)
        {
            if (m_prevMPos == Input.mousePosition)
            {
                rotCounter++;
                if (rotCounter == m_RotRevCooldown)
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
    public void Aim()
    {
        if (m_Rotating)
        {
            var pos = Camera.main.WorldToScreenPoint(m_VehicleTurret.transform.position);
            m_prevMPos = Input.mousePosition;
            var dir = m_prevMPos - pos;
            var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            m_VehicleTurret.transform.rotation = Quaternion.Lerp(m_VehicleTurret.transform.rotation, Quaternion.AngleAxis(angle, transform.up), Time.fixedDeltaTime * m_rotationSpeed);

        }
        else
        {
            m_VehicleTurret.transform.rotation = Quaternion.Lerp(m_VehicleTurret.transform.rotation, transform.rotation, Time.fixedDeltaTime * m_rotationSpeed);
        }


    }

    /// <summary>
    /// Used to fire the hook
    /// </summary>
    public void Fire()
    {
       
        m_Hook.gameObject.SetActive(true);
        m_Hook.transform.rotation = m_VehicleTurret.transform.rotation;
        m_Hook.transform.position = m_spawnPoint;
        m_Hook.Launch(m_VehicleTurret.transform.forward, m_MaxChainLength,m_RetractPointDist,m_LaunchForce,m_ReturnForce);
        m_Chain.CreateRope(m_Hook.gameObject);
    }

    public void Retract()
    {
        m_Hook.gameObject.SetActive(false);
        m_Chain.DestroyRope();
    }

    void Grabbing()
    {
        float distance = Vector3.Distance(transform.position, m_Hook.transform.position);
        if (distance > m_MaxChainLength+2 && m_Hook.gameObject.activeSelf)
        {
            Vector3 heading = m_Hook.transform.position - transform.position;
            Vector3 axis = Vector3.Cross(m_rb.velocity, heading); // orbit axis
            Vector3 direction = Vector3.Cross(heading, axis).normalized;
            Quaternion newRot = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot,Time.fixedDeltaTime * m_angularRotationFactor);

            m_rb.velocity = direction * m_rb.velocity.magnitude;
        }
    }

    void Update()
    {
        m_spawnPoint = new Vector3(m_VehicleTurret.transform.position.x, m_VehicleTurret.transform.position.y, m_VehicleTurret.transform.position.z + m_spawnPointOffset);
        m_Hook.spawnPosition = m_spawnPoint;

    }

    void FixedUpdate()
    {
        Grabbing();
    }
	
}
