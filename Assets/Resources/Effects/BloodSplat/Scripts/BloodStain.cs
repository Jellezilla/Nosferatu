using UnityEngine;
using System.Collections;

public class BloodStain : MonoBehaviour {

    public Material[] materials;
    private Material _myMaterial;

	void Start () {
        //Debug.Log(materials.Length - 1);
        _myMaterial = materials[Random.Range(0,materials.Length)];
        gameObject.GetComponent<Renderer>().material = _myMaterial;
        //Debug.Log(_myMaterial);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
