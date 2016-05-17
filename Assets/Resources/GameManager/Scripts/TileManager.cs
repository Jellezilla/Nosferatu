using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    [SerializeField]
    private Vector3 m_LevelOrigin;
    [SerializeField]
    private float m_tileChangeDistance;
    [SerializeField]
    private GameObject m_startTilePrefab;
    private Tile m_startTile;
    [SerializeField]
    private GameObject[] m_easyTilePrefabs;
    [SerializeField]
    private GameObject[] m_normalTilesPrefabs;
    [SerializeField]
    private GameObject[] m_hardTilesPrefabs;
    private Tile[][] m_Tiles;
    private GameObject m_Player;
    private int m_prevTileIndex;
    private int m_curTileIndex;
    private int m_curTileDiffIndex;
    private int m_prevTileDiffIndex;
    private Collider m_curTileCol;
    private Collider m_prevTileCol;
    private float m_carHeight;
    private const int m_nrOfDiffs = 3;
    void Start()
    {
        Init();
        StartTile();
    }

    /// <summary>
    /// Used to setup the instantiate and setup the tiles
    /// </summary>
    private void Init()
    {
        m_Player = GameController.Instance.Player;
        m_startTile = Instantiate(m_startTilePrefab).GetComponent<Tile>();
        m_startTile.gameObject.SetActive(false);
        m_Tiles = new Tile[m_nrOfDiffs][];

        for (int i = 0; i < m_nrOfDiffs; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        m_Tiles[i] = new Tile[m_easyTilePrefabs.Length];
                        for (int j = 0; j < m_easyTilePrefabs.Length; j++)
                        {
                            m_Tiles[i][j] = Instantiate(m_easyTilePrefabs[j]).GetComponent<Tile>();
                            m_Tiles[i][j].gameObject.SetActive(false);
                        }
                        break;
                    }

                case 1:
                    {
                        m_Tiles[i] =  new Tile[m_normalTilesPrefabs.Length];
                        for (int j = 0; j < m_normalTilesPrefabs.Length; j++)
                        {
                            m_Tiles[i][j] = Instantiate(m_normalTilesPrefabs[j]).GetComponent<Tile>();
                            m_Tiles[i][j].gameObject.SetActive(false);
                        }
                        break;
                    }

                case 2:
                    {
                        m_Tiles[i] = new Tile[m_hardTilesPrefabs.Length];
                        for (int j = 0; j < m_hardTilesPrefabs.Length; j++)
                        {
                            m_Tiles[i][j] = Instantiate(m_hardTilesPrefabs[j]).GetComponent<Tile>();
                            m_Tiles[i][j].gameObject.SetActive(false);
                        }
                        break;
                    }
            }
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
        m_startTile.transform.position = m_LevelOrigin;
        m_curTileCol = m_startTile.GetComponent<Collider>();
        m_prevTileCol = m_curTileCol;
        m_startTile.TileLoader();
        Vector3 tileCenter = m_curTileCol.bounds.center;
        m_carHeight = m_Player.GetComponent<Collider>().bounds.extents.y * 2;
        tileCenter.y += m_carHeight;
        m_Player.transform.position = tileCenter; // must replace with spawnPoint in new tiles
    }

    private void TileChanger()
    {
        Vector3 playerOnTile = m_Player.transform.position;
        playerOnTile.y = m_curTileCol.transform.position.y;
        if (m_curTileCol.bounds.Contains(playerOnTile) && playerOnTile.z > m_curTileCol.bounds.center.z)
        {
            if (!m_prevTileCol.bounds.Contains(playerOnTile))
            {
                if (m_prevTileCol.gameObject.GetInstanceID() == m_startTile.gameObject.GetInstanceID())
                {
                    m_startTile.TileUnloader();
                }
                else
                {
                    m_Tiles[m_prevTileDiffIndex][m_prevTileIndex].TileUnloader();
                }


            }
            m_prevTileCol = m_curTileCol;
            m_prevTileDiffIndex = m_curTileDiffIndex;
            m_prevTileIndex = m_curTileIndex;
             Vector3 nTilePos = m_curTileCol.transform.position + new Vector3(0, 0, m_curTileCol.bounds.extents.z * 2);

            m_curTileDiffIndex = Random.Range(0, m_nrOfDiffs); // testing line

            switch (m_curTileDiffIndex)
            {
                case 0:
                    {
                        do
                        {
                            m_curTileIndex = Random.Range(0, m_Tiles[m_curTileDiffIndex].Length);
                        }
                        while (m_Tiles[m_curTileDiffIndex][m_curTileIndex].gameObject.activeSelf);
                        break;
                    }
                case 1:
                    {
                        do
                        {
                            m_curTileIndex = Random.Range(0, m_Tiles[m_curTileDiffIndex].Length);
                        }
                        while (m_Tiles[m_curTileDiffIndex][m_curTileIndex].gameObject.activeSelf);
                        break;
                    }
                case 2:
                    {
                        do
                        {
                            m_curTileIndex = Random.Range(0, m_Tiles[m_curTileDiffIndex].Length);
                        }
                        while (m_Tiles[m_curTileDiffIndex][m_curTileIndex].gameObject.activeSelf);
                        break;
                    }
            }
            m_Tiles[m_curTileDiffIndex][m_curTileIndex].transform.position = nTilePos;
            m_Tiles[m_curTileDiffIndex][m_curTileIndex].TileLoader();
            m_curTileCol = m_Tiles[m_curTileDiffIndex][m_curTileIndex].GetComponent<Collider>();

        }
    }


}
