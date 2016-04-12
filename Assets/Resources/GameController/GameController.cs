using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Gamecontroller class, Usage GameController.Instance
/// </summary>
public class GameController : Singleton<GameController>
{

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

    public GameObject Player
    {
        get
        {
            return m_player;
        }
    }

}
