using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour {

    private Animator anim;
    private GameObject player;
    private bool _isDead = false; //used for creating bloodspatter only on first hit
    public GameObject bloodSpatterObject;
    public GameObject TESTuimanager;
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
    private Rigidbody[] m_rbs;
    private Collider[] m_cols;
    private CharacterJoint[] m_joints;

   
    // Use this for initialization
    void Awake () {
        Init();
        
	}
	

    /// <summary>
    ///  Initialize references and variables
    /// </summary>
	void Init()
    {
        anim = GetComponent<Animator>();
        m_rbs = GetComponentsInChildren<Rigidbody>();
        m_cols = GetComponentsInChildren<Collider>();


        for (int i = 0; i < m_rbs.Length; i++)
        {
            if (i != 0)
            {
                if (i == 1)
                {
                    m_cols[i].enabled = true;
                }
                else
                {
                    m_cols[i].enabled = false;
                }

                m_rbs[i].useGravity = false;
                m_rbs[i].isKinematic = true;
                m_rbs[i].Sleep();
            }

        }
        
   
    }




    /// <summary>
    ///  Controls the collision with the player (car) 
    /// </summary>
    /// <param name="col"></param>
    /*
    void OnTriggerEnter(Collider col) 
    {
        if (col.tag == Tags.player && !_isDead)
        {
            Debug.Log("hit");

            GoRagdoll();
            _isDead = true;
          
        }
    }
    */
    void OnTriggerEnter(Collider col)
    {
       
        if (col.tag == Tags.player && !_isDead)
        {
//            Debug.Log("hit");
            //  CapsuleCollider theCol = GetComponent<CapsuleCollider>();
            //  theCol.enabled = false;
            for (int i = 0; i < m_rbs.Length; i++)
            {
                if (i != 0)
                {
                    if (i == 1)
                    {
                        m_cols[i].enabled = false;
                        //acount for root object collider
                    }
                    else
                    {
                        m_cols[i].enabled = true;
                    }
                    m_rbs[i].useGravity = true;
                    m_rbs[i].isKinematic = false;
                    m_rbs[i].WakeUp();
                }
                else //if its equal to 0
                {
                    m_cols[i].enabled = false;
                    m_rbs[i].useGravity = false;
                    m_rbs[i].Sleep();
                }


            }
            GoRagdoll();
            _isDead = true;
        }
    }

    /// <summary>
    /// This method is run when hit by the car. It will make the blood splatter, make the human collapse (ragdoll effect) and call destroy on it.
    /// </summary>
    void GoRagdoll()
    {
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
        (Instantiate(bloodSpatterObject, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation) as GameObject).transform.parent = transform;

        // Destroy object after 5 seconds. 
        StopAllCoroutines();
        Destroy(gameObject, 5);
        
    }

    

    void FixedUpdate()
    {
        // When car comes close, the humans panic
     /*   if (Vector3.Distance(transform.position, player.transform.position) < panicRange)
        {

            Panic();
        }*/
        
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
