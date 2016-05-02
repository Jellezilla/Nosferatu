using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

    [SerializeField]
    private Vector3 m_LevelOrigin;
    [SerializeField]
    private float m_tileChangeDistance;
    [SerializeField]
    private GameObject m_startTilePrefab;
    private GameObject m_startTile;
    [SerializeField]
    private GameObject m_rampageTilePrefab;
    private GameObject m_rampageTile;
    [SerializeField]
    private GameObject[] m_easyTilePrefabs;
    private GameObject[] m_easyTiles;
    [SerializeField]
    private GameObject[] m_normalTilesPrefabs;
    private GameObject[] m_normalTiles;
    [SerializeField]
    private GameObject[] m_hardTilesPrefabs;
    private GameObject[] m_hardTiles;
    private GameObject m_Player;
    private GameObject m_prevTile;
    private GameObject m_curTile;
    private Collider m_curTileCol;
    private Collider m_prevTileCol;
    private float m_carHeight;

    void Start()
    {
        Init();
        StartTile();
    }

    private void Init()
    {
        m_Player = GameController.Instance.Player;
        m_startTile = Instantiate(m_startTilePrefab);
        m_startTile.SetActive(false);
        m_rampageTile = Instantiate(m_rampageTilePrefab);
        m_rampageTile.SetActive(false);

        m_easyTiles = new GameObject[m_easyTilePrefabs.Length];
        for (int i = 0; i < m_easyTilePrefabs.Length; i++)
        {
            m_easyTiles[i] = Instantiate(m_easyTilePrefabs[i]);
            m_easyTiles[i].SetActive(false);
        }

        m_normalTiles = new GameObject[m_normalTilesPrefabs.Length];
        for (int i = 0; i < m_normalTilesPrefabs.Length; i++)
        {
            m_normalTiles[i] = Instantiate(m_normalTilesPrefabs[i]);
            m_normalTiles[i].SetActive(false);
        }

        m_hardTiles = new GameObject[m_hardTilesPrefabs.Length];
        for (int i = 0; i < m_hardTilesPrefabs.Length; i++)
        {
            m_hardTiles[i] = Instantiate(m_hardTilesPrefabs[i]);
            m_normalTiles[i].SetActive(false);
        }

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
        // m_curTileIndex = m_startTileIndex;
        m_curTile = m_startTile;
        m_curTileCol = m_curTile.GetComponent<Collider>();
        m_curTile.GetComponent<Tile>().TileLoader();
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
            if (m_prevTile != null && !m_prevTileCol.bounds.Contains(playerOnTile))
            {
                m_prevTile.GetComponent<Tile>().TileUnloader();
            }
            m_prevTile = m_curTile;
            m_prevTileCol = m_curTileCol;
            Vector3 nTilePos = m_curTile.transform.position + new Vector3(0, 0, m_curTileCol.bounds.extents.z * 2);

            int tileDifficulty = Random.Range(0,3); // testing line
            switch (tileDifficulty)
            {
                case 0:
                    {

                        for (int i = 0; i < m_easyTiles.Length; i++)
                        {
                            if (m_easyTiles[i].activeSelf==false)
                            {
                                m_curTile = m_easyTiles[i];
                                break;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < m_normalTiles.Length; i++)
                        {
                            if (m_normalTiles[i].activeSelf == false)
                            {
                                m_curTile = m_normalTiles[i];
                                break;
                            }
                        }
                        break;
                    }

                case 2:
                    {
                        for (int i = 0; i < m_hardTiles.Length; i++)
                        {
                            if (!m_hardTiles[i].activeSelf==false)
                            {
                                
                                m_curTile = m_hardTiles[i];
                                break;
                            }
                        }
                        break;
                    }
            }

            m_curTile.transform.position = nTilePos;
            m_curTile.GetComponent<Tile>().TileLoader();
            m_curTileCol = m_curTile.GetComponent<Collider>();

        }
    }


}
