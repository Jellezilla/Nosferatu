using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

    [SerializeField]
    private Vector3 m_LevelOrigin;
    [SerializeField]
    private float m_tileChangeDistance;
    [SerializeField]
    private int m_startTileIndex;
    [SerializeField]
    private int m_easyTileIndex;
    [SerializeField]
    private int m_mediumTileIndex;
    [SerializeField]
    private int m_hardTileIndex;
    [SerializeField]
    private int m_rampageTileIndex;
    private GameObject m_Player;
    private GameObject m_prevTile;
    private GameObject m_curTile;
    private GameObject m_nextTile;
    private Collider m_cTileCol;
    private Collider m_nTileCol;

    void Start()
    {
        Init();
        StartTile();
    }

    private void Init()
    {
        m_Player = GameController.Instance.Player;

    }

    /// <summary>
    /// Create first tile
    /// </summary>
    private void StartTile()
    {
        m_curTile = GameController.Instance.ObjectPool.GrabObject(1, m_LevelOrigin, Quaternion.identity);
        m_cTileCol = m_curTile.GetComponent<Collider>();
        Vector3 tileCenter = m_cTileCol.bounds.center;
        float carHeight = m_Player.GetComponent<Collider>().bounds.extents.y * 2;
        tileCenter.y = carHeight;
        m_Player.transform.position = tileCenter; 
    }

    private void TileChanger()
    {
        if (Vector3.Distance(m_Player.transform.position, m_cTileCol.bounds.center) < m_tileChangeDistance)
        {

        }
    }


}
