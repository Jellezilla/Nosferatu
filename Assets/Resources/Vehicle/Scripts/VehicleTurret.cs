using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(TurretedVehicleUserControl))]
public class VehicleTurret : MonoBehaviour {

    [SerializeField]
    private GameObject m_VehicleTurret;
    [SerializeField]
    private float spawnPointOffset;
    private Vector3 m_spawnPoint;
    private float m_OldRotation;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    private Quaternion m_originalRot;
    private Vector3 m_prevMPos;
    [SerializeField]
    private int m_RotRevCooldown;
    private WaitForSeconds m_WaitStep;
    private bool m_Rotating;
	// Use this for initialization
	void Start () {

        m_originalRot = m_VehicleTurret.transform.rotation;
        m_prevMPos = Input.mousePosition;
        m_WaitStep = new WaitForSeconds(1);
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
                    if (m_VehicleTurret.transform.rotation == m_originalRot)
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
            m_VehicleTurret.transform.rotation = Quaternion.Lerp(m_VehicleTurret.transform.rotation, m_originalRot, Time.fixedDeltaTime * m_rotationSpeed);
        }


    }

    /// <summary>
    /// Used to fire the grapple hook
    /// </summary>
    public void Fire()
    {
        throw new NotImplementedException();
    }
	
}
