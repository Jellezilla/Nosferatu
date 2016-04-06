using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

    private Animator anim;
    private GameObject player;

    [SerializeField]
    private float speed = 0.2f;
    [SerializeField]
    private float panicSpeed = 0.6f;
    [SerializeField]
    private float panicTime = 2.5f;
    [SerializeField]
    private float crippleSpeed = 0.15f;
    [SerializeField]
    private float panicRange = 5.0f;

   
    // Use this for initialization
    void Start () {
        Init();
        
	}
	

    /// <summary>
    ///  Initialize references and variables
    /// </summary>
	void Init()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");

   
    }




    /// <summary>
    ///  Controls the collision with the player (car) 
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col) 
    {
        if (col.tag == "Player") 
        {
            GoRagdoll();
        }
    }
       /// <summary>
    /// This method is run when hit by the car. It will make the blood splatter, make the human collapse (ragdoll effect) and call destroy on it.
    /// </summary>
    void GoRagdoll()
    {
        Debug.Log("go ragdoll!");
        if (Random.Range(1, 15) == 1)
        {
            //CreateCripple();
            //return;
        }
        
        anim.enabled = false;
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        
        // setting all rigidbodies in the human to isKinematic = false, creates the ragdoll effect and collapses him/her.         
        foreach(Rigidbody rb in rigids)
        {
            rb.isKinematic = false;
        }


        // play blood splatter effect




        // Destroy object after 5 seconds. 
        StopAllCoroutines();
        Destroy(gameObject, 5);
        
    }

    

    void FixedUpdate()
    {


        // When car comes close, the humans panic
        if (Vector3.Distance(transform.position, player.transform.position) < panicRange)
        {

            Panic();
        }
        
    }
   
    void Idle()
    {
        
        anim.SetFloat("speed", 0.0f);
        
    }
    
  

    void Panic()
    {
        

        anim.SetBool("panic", true);
        // move
        
        StartCoroutine(calmDown(panicTime));
      
    }
 
    

    IEnumerator calmDown(float wait)
    {
        
        yield return new WaitForSeconds(wait);
        anim.SetBool("panic", false);
              

    }
    
}
