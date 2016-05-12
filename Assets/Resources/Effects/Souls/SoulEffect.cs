using UnityEngine;
using System.Collections;

public class SoulEffect : MonoBehaviour {
    //Stolen from  unity3d.com
    private Transform _startMarker;
    public Transform endMarker;
    private Transform _target;
    private float speed = 5.0F;
    private float startTime;
    private float journeyLength;
  //  Vector3 tpos = new Vector3(0, 0, 0);

    void Start() {
        _startMarker= transform;
        _target = endMarker.Find("SpBar").transform;

        startTime = Time.time;
        journeyLength = Vector3.Distance(_startMarker.position,_target.position);
        _target = Camera.main.transform.GetChild(0).GetChild(0).Find("HUD").Find("SpBar").transform;
    }

    void Update() {

        
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        if (Vector3.Distance(transform.position, _target.position) > 1) {
            transform.position = Vector3.Lerp(_startMarker.position, _target.position, fracJourney);
        }
        else {
           Destroy(gameObject);
        }
    }
}
