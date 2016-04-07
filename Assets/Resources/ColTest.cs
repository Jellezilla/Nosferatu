using UnityEngine;
using System.Collections;

public class ColTest : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
