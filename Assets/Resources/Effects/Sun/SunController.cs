using UnityEngine;
using System.Collections;

public class SunController : MonoBehaviour {
    //ALEX DONT FREAK OUT, THIS PIECE OF CODE IS DIRTY-WIP AS F!

    public float CloseLimit = 30, FarLimit = 50;
    
    private float _distanceToSun=20;
    private float _distanceTraveled=0;
    private Light _dirLight;
    private Light _spotLight;

    void Start () {
        _spotLight = transform.GetComponent<Light>();
        _dirLight = transform.GetChild(0).GetComponent<Light>();
        _distanceTraveled = 0;// transform.position.z-_distanceToSun;
    }
	
	void Update () {
        _distanceToSun = Camera.main.transform.position.z - transform.position.z -_distanceTraveled;//_distanceTraveled;
        //Handling the distance to sun
        if (_distanceToSun > CloseLimit) {
            _distanceTraveled += 0.0004f;
            Debug.Log("moving sun");
        }
        else {
            Debug.Log("Now you are dead");
            _distanceTraveled = 0;
            _distanceToSun = CloseLimit;

        }

        if (_distanceToSun > FarLimit) {
            Debug.Log("Sun catching up");
            _distanceToSun = FarLimit;
            //_distanceTraveled = 0;
        }

        //Changing directional light intensity when sun is closer
        float totalDist = FarLimit - CloseLimit; //((max-current)/max/2;
        float newIntensity = 0.5f + ((totalDist - (_distanceToSun-CloseLimit))/totalDist);
        _dirLight.intensity = newIntensity;
        Debug.Log(newIntensity);

        if (_distanceToSun < CloseLimit + 15) {
            _spotLight.enabled = true;
        }
        else {
            _spotLight.enabled = false;
        }

            //if (_dirLight.intensity > 0.5) {
            //    float totalDist = FarLimit - CloseLimit;
            //    _dirLight.intensity = 0.5f + totalDist - (_distanceToSun - CloseLimit) / totalDist / 2;
            //}
            //else {
            //    _dirLight.intensity = 0.51f;
            //}


            Vector3 newPos = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z-_distanceToSun);
        transform.position = newPos;
        
    }
}
