using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSpatter : MonoBehaviour {

    private ParticleSystem ps;
    public Transform PaintPrefab;

    private int MinSplashs = 25;
    private int MaxSplashs = 40;
    private float SplashRange = 2f;

    private float MinScale = 0.25f;
    private float MaxScale = 2.5f;

    // DEBUG
//    private bool mDrawDebug;
//    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();

    public void Start() {
        ps = GetComponent<ParticleSystem>();

        Ray ray = Camera.main.ScreenPointToRay(transform.position);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            // Paint!
            // Step back a little for a better effect (that's what "normal * x" is for)
            Debug.Log(hit.transform.position);
            Paint(transform.position + hit.normal * (SplashRange / 4f));
        }
    }

    public void Update() {
        if (ps) {
            if (!ps.IsAlive()) {
                Destroy(gameObject);
            }
        }
    }

    public void Paint(Vector3 location) {
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
                // Create a splash if we found a surface
                var paintSplatter = GameObject.Instantiate(PaintPrefab,
                                                           hit.point,

                                                           // Rotation from the original sprite to the normal
                                                           // Prefab are currently oriented to z+ so we use the opposite
                                                           Quaternion.FromToRotation(Vector3.back, hit.normal)
                                                           ) as Transform;

                // Random scale
                var scaler = Random.Range(MinScale, MaxScale);

                paintSplatter.localScale = new Vector3(
                    paintSplatter.localScale.x * scaler,
                    paintSplatter.localScale.y * scaler,
                    paintSplatter.localScale.z
                );

                // Random rotation effect
                var rater = Random.Range(0, 359);
                paintSplatter.transform.RotateAround(hit.point, hit.normal, rater);
                paintSplatter.SetParent(hit.transform);

                // TODO: What do we do here? We kill them after some sec?
                Destroy(paintSplatter.gameObject, 3);
            }

        }
    }
}
