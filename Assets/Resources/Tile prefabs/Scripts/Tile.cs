using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    private bool m_initTile;
    [SerializeField]
    private GameObject m_Humans;
    [SerializeField]
    private GameObject m_tombstones;
    // Use this for initialization

    void OnEnable()
    {
        if (!m_initTile)
        {

        }
    }
    
    void Awake ()
    {
        if (!m_initTile)
        {

        }

	}

    void InitTile()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
