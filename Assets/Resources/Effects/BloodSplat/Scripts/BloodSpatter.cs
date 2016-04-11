using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BloodSpatter : MonoBehaviour {

    private ParticleSystem ps;
    public Transform PaintPrefab;

    private int MinSplashs = 5;
    private int MaxSplashs = 10;
    private float SplashRange = 3f;

    private float MinScale = 0.8f;
    private float MaxScale = 3.5f;

    private GameObject _spatterDecal;
    // DEBUG
//    private bool mDrawDebug;
//    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();
    public void Awake() {
        gameObject.transform.Rotate(60, 0, 0, Space.Self);
        // gameObject.transform.rotation = Quaternion.Euler(0f, 0f, -60f);
    }
    public void Start() {
       // ps = GetComponent<ParticleSystem>();

        Ray ray = Camera.main.ScreenPointToRay(transform.position);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            // Paint!
            // Step back a little for a better effect (that's what "normal * x" is for)
            Debug.Log(hit.transform.position);
            //Paint(transform.position + hit.normal * (SplashRange / 4f));
            Vector3 dist = transform.position+transform.forward*1.05f;
            
//            CreateBloodSpatter(new Vector3(dist.x, transform.position.y-1,dist.z) + hit.normal * (SplashRange / 4f));
            CreateBloodSpatter(new Vector3(dist.x, transform.position.y - 1.5f, dist.z) + hit.normal * (SplashRange / 4f));
            //  Paint(new Vector3(dist.x, dist.y - 2, dist.z) + hit.normal * (SplashRange / 4f));
        }

        Destroy(gameObject,4);
    }

    public void Update() {
        //if (ps) {
        //    if (!ps.IsAlive()) {
        //        Destroy(gameObject);
        //    }
        //}
    }



    public void CreateBloodSpatter(Vector3 location) {
        //DEBUG
        //mHitPoint = location;
        //mRaysDebug.Clear();
        //mDrawDebug = true;

        int n = -1;

        int drops = Random.Range(MinSplashs, MaxSplashs);
        RaycastHit hit;

        // Generate multiple decals in once
        while (n <= drops) {
            n++;

            // Get a random direction (beween -n and n for each vector component)
            var fwd = transform.TransformDirection(Random.onUnitSphere * SplashRange);

            mRaysDebug.Add(new Ray(location, fwd));

            // Raycast around the position to splash everwhere we can
            if (Physics.Raycast(location, fwd, out hit, SplashRange)) {
                if (hit.collider.gameObject.tag == Tags.environment) {
                    // Create a splash if we found a surface
                    Debug.Log("name"+hit.collider.gameObject.name);
                    var paintSpatter = GameObject.Instantiate(PaintPrefab,
                                                               hit.point,

                                                               // Rotation from the original sprite to the normal
                                                               // Prefab are currently oriented to z+ so we use the opposite
                                                               //Quaternion.FromToRotation(Vector3.back, hit.normal)
                                                               Quaternion.LookRotation(fwd)
                                                               ) as Transform;


                    // Random scale
                    var scaler = Random.Range(MinScale, MaxScale);

                    paintSpatter.localScale = new Vector3(
                        paintSpatter.localScale.x * scaler,
                        paintSpatter.localScale.y * scaler,
                        paintSpatter.localScale.z *scaler
                    );

                    // Random rotation effect
                    var rater = Random.Range(0, 359);

                    //fixed rotation to make look at correct angle for deferred decal reprensentation
                    paintSpatter.transform.RotateAround(hit.point, hit.normal, rater);

                   // if (paintSpatter.position.y > transform.position.y-2)
                        //paintSpatter.Rotate(270, 0, 0, Space.Self);
//                    else paintSpatter.rotation = Quaternion.identity;

                    _spatterDecal = paintSpatter.gameObject;

                    // TODO: What do we do here? We kill them after some sec?
                    Destroy(_spatterDecal.gameObject, 3);
                }
            }

        }
       // SceneView.RepaintAll();
        //SceneView.RepaintAll();
    }
}
