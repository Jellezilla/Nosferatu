using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

    [SerializeField]
    private Vector3 m_LevelOrigin;
    private GameObject m_Player;

    void Start()
    {
        Init();
    }

    void Init()
    {
        m_Player = GameController.Instance.Player;

    }



}
