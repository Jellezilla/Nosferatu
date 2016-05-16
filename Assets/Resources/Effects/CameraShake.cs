using UnityEngine;
 using System.Collections;
 
 public class CameraShake : MonoBehaviour {

    private Vector3 originPosition;
    private Quaternion originRotation;
    private Quaternion standardRotation;
    public float shake_decay;
    public float shake_intensity;

    void OnEnable(){
        standardRotation = transform.rotation;
        ShakeCamera();
    }

    void Update() {
        ShakeCamera();
        if (shake_intensity > 0) {
            transform.localPosition = originPosition + Random.insideUnitSphere * shake_intensity;
            transform.localRotation = new Quaternion(
                originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .02f,
                originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .02f,
                originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .02f,
                originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .02f);
            shake_intensity -= shake_decay;
        }
        else  {
            transform.rotation = standardRotation;
            Debug.Log("end");
        }
    }

    public void ShakeCamera() {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }
}