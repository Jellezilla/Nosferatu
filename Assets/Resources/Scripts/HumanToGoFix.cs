using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class HumanToGoFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
[CustomEditor(typeof(HumanToGoFix))]
class SunSystemBuilder : Editor {
        //SolarSystemGen ssg = GameObject.FindWithTag ("God").GetComponent<SolarSystemGen>();  // new SolarSystemGen();
       

    public override void OnInspectorGUI() {
        if (GUILayout.Button ("I am God!")) {
            //ssg.counter++;
            
            //ssg.GenerateSolarSystem(ssg.planetCount, ssg.counter);
           // ssg.GenerateSolarSystem(8, 4);
            Debug.Log("It's alive: " + target.name);



            
            GameObject[] humanz = GameObject.FindGameObjectsWithTag("Human");
            Debug.Log(humanz.Length);

                

                foreach(GameObject go in humanz)
                {
                    GameObject placeHolder = new GameObject();
                    //Instantiate(placeHolder, go.transform.position, go.transform.rotation);

                    placeHolder.transform.position = go.transform.position;
                    placeHolder.name = "human";
                    placeHolder.tag = "Human";
                    

                }
        }

    }
}

}


