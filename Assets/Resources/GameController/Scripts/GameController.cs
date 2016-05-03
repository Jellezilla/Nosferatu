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
    private Rigidbody m_playerRB;
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
        m_playerRB = m_player.GetComponent<Rigidbody>();
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

    public Rigidbody PlayerRigidBody
    {
        get
        {
            return m_playerRB;
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

    public bool OutOfFuel {
        get {
            return m_playerRes.OutOfFuel;
        }
    }

    public bool PlayerDead
    {
        get
        {
            return m_playerRes.DeadInTheWater;
        }
    }
}
