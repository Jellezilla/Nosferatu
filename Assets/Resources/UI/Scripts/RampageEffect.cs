using UnityEngine;
using System.Collections;

public class RampageEffect : MonoBehaviour {
    private Animator _rampageAnimator;
	// Use this for initialization
	void Start () {
      //  _rampageAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EndAnimation() {
        gameObject.SetActive(false);
    }

    public void ResetAnimation(){
         
    }
}
