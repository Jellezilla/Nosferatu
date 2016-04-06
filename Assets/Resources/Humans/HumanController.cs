using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

    private Animator anim;
    private GameObject player;
    private bool _isDead = false; //used for creating bloodspatter only on first hit
    public GameObject bloodSpatterObject;
    [SerializeField]
    private float panicTime = 2.5f;

    private bool isCripple = false;

    
	// Use this for initialization
	void Start () {
        Init();
	}
	
	void Init()
    {
        anim = GetComponent<Animator>();

        
        player = GameObject.FindWithTag("Player");

    }

    void OnTriggerEnter(Collider col) 
    {
      /**  if (col.tag == "Player") 
        {
            GoRagdoll();
        }*/

        if (col.gameObject.name == "Jeep" && !_isDead)
        {
            GoRagdoll();
            _isDead = true;
            Instantiate(bloodSpatterObject, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), col.gameObject.transform.rotation);
            Debug.Log("HIT HIM");
        }
    }
    
    /// <summary>
    /// This method is run when hit by the car. It will make the blood splatter, make the human collapse (ragdoll effect) and call destroy on it.
    /// </summary>
    void GoRagdoll()
    {

        if (Random.Range(1, 15) == 1)
        {
            CreateCripple();
            return;
        }


        anim.enabled = false;
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
                
        foreach(Rigidbody rb in rigids)
        {
            rb.isKinematic = false;
        }

        
        // play blood splatter effect




       // Destroy object after 5 seconds. 
        Destroy(gameObject, 5);
        
    }

    void CreateCripple()
    {
        isCripple = true;
        anim.SetBool("cripple", true);
        anim.SetBool("panic", false);
        anim.SetFloat("speed", 0.0f);

        Destroy(gameObject, 15);
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 5.0f)
        {
            Panic();
        }
       

    }
    

    void Wander()
    {
        anim.SetFloat("speed", 1.1f);
    }

    void Idle()
    {
        anim.SetFloat("speed", 0.0f);
    }

    void Panic()
    {
        anim.SetBool("panic", true);
        StartCoroutine(calmDown(panicTime));
    }



    IEnumerator calmDown(float wait)
    {

        yield return new WaitForSeconds(wait);
        anim.SetBool("panic", false);

    }

    



}
