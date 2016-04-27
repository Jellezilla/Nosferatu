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
    private int m_curTileIndex;
    private int m_prevTileIndex;
    private Collider m_curTileCol;

    void Start()
    {
        Init();
        StartTile();
    }

    private void Init()
    {
        m_Player = GameController.Instance.Player;

    }


    void Update()
    {
        TileChanger();
    }

    /// <summary>
    /// Create first tile
    /// </summary>
    private void StartTile()
    {
        m_curTileIndex = m_startTileIndex;
        m_curTile = GameController.Instance.ObjectPool.GrabObject(m_curTileIndex, m_LevelOrigin, Quaternion.identity);
        m_curTileCol = m_curTile.GetComponent<Collider>();
        Vector3 tileCenter = m_curTileCol.bounds.center;
        float carHeight = m_Player.GetComponent<Collider>().bounds.extents.y * 2;
        tileCenter.y = carHeight;
        m_Player.transform.position = tileCenter; 
    }

    private void TileChanger()
    {
        if (Vector3.Distance(m_Player.transform.position, m_curTileCol.bounds.center) < m_tileChangeDistance)
        {
            if (m_prevTile != null)
            {
                GameController.Instance.ObjectPool.ReturnObject(m_prevTileIndex, m_prevTile);
            }
            Vector3 nTilePos = m_curTile.transform.position + new Vector3(0, 0, m_curTileCol.bounds.extents.z * 2);
            m_prevTileIndex = m_curTileIndex;
            m_prevTile = m_curTile;
            m_curTileIndex = Random.Range(m_easyTileIndex,m_hardTileIndex+1); // testing line
            m_curTile = GameController.Instance.ObjectPool.GrabObject(m_curTileIndex, nTilePos, Quaternion.identity);
            m_curTileCol = m_curTile.GetComponent<Collider>();

        }
    }


}
