using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Gamecontroller class, Usage GameController.Instance
/// </summary>
public class GameController : Singleton<GameController>
{
    private float _fuelCurrent=100,_fuelMax=100, _soulsCurrent=0,_soulsMax=100;
    private GameObject m_player;
    // Use this for initialization
    void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag(Tags.player);
    }

    void OnLevelWasLoaded(int level)
    {
        m_player = GameObject.FindGameObjectWithTag(Tags.player);
    }

    void Update() {
        _fuelCurrent -= 0.2f;
        if (_fuelCurrent < 0.1) _fuelCurrent = 0.1f;
            Debug.Log("running");
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
            return _fuelCurrent;
        }
    }

    public float GetSouls {
        get {
            return _soulsCurrent;
        }
    }

    public void AddFuel() {
        _fuelCurrent += 2;
        if (_fuelCurrent > _fuelMax) {
            _fuelCurrent = _fuelMax;
        }
    }

    public void AddSoul() {
        _soulsCurrent += 10;
        if (_soulsCurrent > _soulsMax) {
            _soulsCurrent = _soulsMax;
        }
    }

}
