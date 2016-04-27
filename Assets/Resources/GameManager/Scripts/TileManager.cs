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
    private int[] m_easyTileIndexs;
    [SerializeField]
    private int[] m_mediumTileIndexs;
    [SerializeField]
    private int[] m_hardTileIndexs;
    [SerializeField]
    private int m_rampageTileIndex;
    private GameObject m_Player;
    private GameObject m_prevTile;
    private GameObject m_curTile;
    private int m_curTileIndex;
    private int m_prevTileIndex;
    private Collider m_curTileCol;
    private float m_carHeight;
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
        m_carHeight = m_Player.GetComponent<Collider>().bounds.extents.y * 2;
        tileCenter.y = m_carHeight;
        m_Player.transform.position = tileCenter; 
    }

    private void TileChanger()
    {
        Vector3 playerOnTile = m_Player.transform.position;
        playerOnTile.y -= m_carHeight;
        if (m_curTileCol.bounds.Contains(playerOnTile) && playerOnTile.z > m_curTileCol.bounds.center.z)
        {

            if (m_prevTile != null)
            {
                GameController.Instance.ObjectPool.ReturnObject(m_prevTileIndex, m_prevTile);
            }
            Vector3 nTilePos = m_curTile.transform.position + new Vector3(0, 0, m_curTileCol.bounds.extents.z * 2);
            m_prevTileIndex = m_curTileIndex;
            m_prevTile = m_curTile;
            int tileDifficulty = Random.Range(0,3); // testing line
            switch (tileDifficulty)
            {
                case 0:
                    {
                        m_curTileIndex = m_easyTileIndexs[Random.Range(0, m_easyTileIndexs.Length)];
                        break;
                    }
                case 1:
                    {
                        m_curTileIndex = m_mediumTileIndexs[Random.Range(0, m_mediumTileIndexs.Length)];
                        break;
                    }

                case 2:
                    {
                        m_curTileIndex = m_hardTileIndexs[Random.Range(0, m_hardTileIndexs.Length)];
                        break;
                    }
            }


            m_curTile = GameController.Instance.ObjectPool.GrabObject(m_curTileIndex, nTilePos, Quaternion.identity);
            m_curTileCol = m_curTile.GetComponent<Collider>();

        }
    }


}
