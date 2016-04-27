using UnityEngine;
using System.Collections;

public class Tombstone : MonoBehaviour {

    private Rigidbody[] m_childrenRbs;
    private BoxCollider[] m_childrenCols;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Init()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hitting tombstone");
        if (collision.gameObject.tag == Tags.player || collision.gameObject.tag == Tags.human)
        {

        }
    }

}
