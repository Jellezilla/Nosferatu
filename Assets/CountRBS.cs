using UnityEngine;
using System.Collections;

public class CountRBS : MonoBehaviour {

    int counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            counter = 0;
            GameObject[] gameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<Rigidbody>() != null)
                {
                    counter++;
                }
            }

            Debug.Log("HOLY FUCK WE GOT : " + counter);
        }
	
	}
}
