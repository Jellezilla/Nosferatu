using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    public GameObject _pObject;
    private Transform _player;
	// Use this for initialization
	void Awake () {

      //  _player = GameController.Instance.Player.transform;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (_pObject != null)
        //if (_player != null)
        {
            gameObject.transform.position = new Vector3(_pObject.transform.position.x, transform.position.y, _pObject.transform.position.z);

            //transform.position = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        }

	
	}
}
