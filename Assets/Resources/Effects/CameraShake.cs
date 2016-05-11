using UnityEngine;
 using System.Collections;
 
 public class CameraShake : MonoBehaviour {

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay;
    public float shake_intensity;

    void Awake(){
        ShakeCamera();
    }
    void Update() {     
        
        if (shake_intensity > 0) {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .02f,
                originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .02f,
                originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .02f,
                originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .02f);
            shake_intensity -= shake_decay;
        }
    }

    public void ShakeCamera() {
        originPosition = transform.position;
        originRotation = transform.rotation;

    }
}