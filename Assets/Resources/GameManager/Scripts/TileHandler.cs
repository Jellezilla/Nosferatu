using UnityEngine;
using System.Collections;
// http://forum.unity3d.com/threads/simple-reusable-object-pool-help-limit-your-instantiations.76851/

public class TileHandler : MonoBehaviour {

    [SerializeField]
    private GameObject currentTile;
    [SerializeField]
    private GameObject nextTile;

    enum TileType { Demo, Regular, Rampage };
    private TileType tile;

    private int demoCount = 0;

    private GameObject player;

    // Use this for initialization
    void Start() {

        player = GameObject.FindWithTag("Player");

        //tile = TileType.Demo;
        tile = TileType.Regular;
        nextTile = GetNextTile();
       
    }
	
	void FixedUpdate()
    {
        if(demoCount > 5 && tile == TileType.Demo)
        {
            tile = TileType.Regular;
        }
        // the + new vector ... 50z) is a hot fix due to the tile prefabs using terrain instead of planes. 
        if(Vector3.Distance(player.transform.position, (nextTile.transform.position + new Vector3(50,0,50))) < Vector3.Distance(player.transform.position, (currentTile.transform.position + new Vector3(50, 0, 50)))) {
            SwitchTiles();
        }
    }



 

    private void SwitchTiles()
    {

       foreach (GameObject go in
       currentTile.GetComponentInChildren<HumanSpawner>().humansInTile)
        {
            Debug.Log("human pooled!" + go.name);
            HumanObjectPool.instance.PoolObject(go);

        }
        // remove old tile (throw it back to the pool) 
     //   ObjectPool.instance.PoolObject(currentTile);

        // Set new current tile
        currentTile = nextTile;

        // Generate next tile
        nextTile = GetNextTile();
        
    }

    private GameObject GetNextTile()
    {
        // Store position of the currentTile tile.
        Vector3 pos = currentTile.transform.position;

        // Get Random tile (string) according to type
        string tileStr = GetRandomTileString();

        // GameObject tilePrefab = ObjectPool.instance.GetObjectForType(tileStr, true);
        //tilePrefab.transform.position = new Vector3(0, 0, 100) + pos;
        return null;// tilePrefab;
    }   
        
    private string GetRandomTileString()
    {
        int rand;
        string str;
        if (tile == TileType.Demo) {
            rand = Random.Range(1, 3);
            str = "Demo" + (rand);
            demoCount++;
        } else if (tile == TileType.Regular) {
            rand = Random.Range(1, 6);
            str = "normal_" + rand;
        } else // rampage
        {
            rand = Random.Range(1, 3);
            str = "Rampage" + rand;
            
        }

        return str;
    }


}
