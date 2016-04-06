using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

    private bool _isDead=false; //used for creating bloodspatter only on first hit
    public GameObject bloodSpatterObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Jeep" && !_isDead) {
            _isDead = true;
            Instantiate(bloodSpatterObject,new Vector3 (transform.position.x,transform.position.y+2,transform.position.z), col.gameObject.transform.rotation);
            Debug.Log("HIT HIM");
        }
    }


}
