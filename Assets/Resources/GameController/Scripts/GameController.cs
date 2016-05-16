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
    private InteractionManager m_interactionM;
    private Rigidbody m_playerRB;
    // Use this for initialization
    void Awake()
    {
        LoadObjectPool();
        LoadPlayer();
        LoadInteractionManager();
    }

    void OnLevelWasLoaded(int level)
    {
        LoadObjectPool();
        LoadPlayer();
        LoadInteractionManager();
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

    private void LoadInteractionManager()
    {
        m_interactionM = GameObject.FindGameObjectWithTag(Tags.interactionManager).GetComponent<InteractionManager>();
    }

    public InteractionManager InteractionManager
    {
        get
        {
            return m_interactionM;
        }
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

    public float MaxFuel
    {
        get
        {
            return m_playerRes.MaxCarBlood;
        }
    }

    public float MaxSouls
    {
        get
        {
            return m_playerRes.MaxCarSouls;
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
