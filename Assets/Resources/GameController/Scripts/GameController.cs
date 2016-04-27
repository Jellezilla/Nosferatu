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
    private ObjectPool m_objPool;
    // Use this for initialization
    void Awake()
    {
        LoadObjectPool();
        LoadPlayer();
    }

    void OnLevelWasLoaded(int level)
    {
        LoadObjectPool();
        LoadPlayer();
    }

    private void LoadPlayer()
    {
        m_player = GameObject.FindGameObjectWithTag(Tags.player);
        m_playerRes = m_player.GetComponent<VehicleResources>();
    }

    private void LoadObjectPool()
    {
        m_objPool = GameObject.FindGameObjectWithTag(Tags.objectpool).GetComponent<ObjectPool>();
    }

    public GameObject Player
    {
        get
        {
            return m_player;
        }
    }

    public ObjectPool ObjectPool
    {
        get
        {
            return m_objPool;
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
