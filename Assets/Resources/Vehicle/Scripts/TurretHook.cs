using UnityEngine;
using System.Collections;

/// <summary>Grappling hook class, handles grapling hook logic</summary>
public class TurretHook : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _maxDistance;
    private Rigidbody _rb;
    private Vector3 _spawnPosition;
   
    // Use this for initialization
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _spawnPosition = transform.position;
       
        //grab original Position
    }



    void OnCollisionEnter(Collision collision)
    {
       
    }
    /// <summary>Used to launch the hook.</summary>
    /// <param name="player">The player rigidbody component must be passed.</param>
    public void Launch()
    {
        _rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }


    //check if the hook has reached max distance
    void DistanceCheck()
    {
        if (Vector3.Distance(_spawnPosition, transform.position) > _maxDistance)
        {
           
            
        }
    }

    void Update()
    {
        DistanceCheck();
    }
}
