using UnityEngine;
using System.Collections;

public class TestHumanDummy : MonoBehaviour {

    private bool _isDead = false;
    public GameObject bloodSpatterObject;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnCollisionEnter(Collision col) {

        if (col.gameObject.tag == Tags.player && !_isDead) {

            //Only create blood spatter effect once on hit
            _isDead = true;

            //Instantiate blood spatter effect object rotation to the players forward
            Instantiate(bloodSpatterObject, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),col.gameObject.transform.rotation);
            //Destroy(bloodSpatterObject.gameObject, 3.5f);
        }
    }
}
