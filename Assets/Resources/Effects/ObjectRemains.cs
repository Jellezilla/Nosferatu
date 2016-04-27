using UnityEngine;
using System.Collections;

public class ObjectRemains : MonoBehaviour {

    private SphereCollider _sc;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < transform.childCount; i++) {

            float randDir = Random.Range(-6, 6);
            float thrust = (GameController.Instance.Player.GetComponent<Rigidbody>().velocity.magnitude +(randDir))/1.2f;//get velocity of player
           // Debug.Log(thrust);
            GameObject go = transform.GetChild(i).gameObject;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*thrust+new Vector3(randDir,-5+thrust/2,0), ForceMode.Impulse);
        }

       //Destroy(gameObject,3);
       // _sc.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
