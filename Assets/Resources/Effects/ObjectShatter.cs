using UnityEngine;
using System.Collections;

public class ObjectShatter : MonoBehaviour {
    public GameObject Remains;
	// Use this for initialization
	void Start () {
        Debug.Log("its created");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
       Debug.Log("hitting tombstone");
       if (collision.gameObject.tag == Tags.player || collision.gameObject.tag == Tags.human) {
            Debug.Log("hitting tombstone");
            Instantiate(Remains, transform.position, collision.gameObject.transform.rotation);
           Destroy(gameObject);
        }
    }
}
