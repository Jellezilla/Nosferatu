using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Gamecontroller class, Usage GameController.Instance
/// </summary>
public class GameController : Singleton<GameController>
{
    private VehicleResources m_playerRes;
    private GameObject m_player;
    // Use this for initialization
    void Awake()
    {
        LoadPlayer();
    }

    void OnLevelWasLoaded(int level)
    {
        LoadPlayer();
    }

    private void LoadPlayer()
    {
        m_player = GameObject.FindGameObjectWithTag(Tags.player);
        m_playerRes = m_player.GetComponent<VehicleResources>();
    }

    public GameObject Player
    {
        get
        {
            return m_player;
        }
    }

    public float GetFuel {
        get {
            return m_playerRes.CarBlood;
        }
    }

    public float GetSouls {
        get {
            return m_playerRes.CarSouls;
        }
    }



}
