using UnityEngine;
using System.Collections;

public class BloodStain : MonoBehaviour {

    [SerializeField]
    private Material[] m_materials;
    private Material m_myMaterial;

	void Start () {
        //Debug.Log(materials.Length - 1);
        m_myMaterial = m_materials[Random.Range(0,m_materials.Length)];
        gameObject.GetComponent<Renderer>().material = m_myMaterial;
        //Debug.Log(_myMaterial);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
