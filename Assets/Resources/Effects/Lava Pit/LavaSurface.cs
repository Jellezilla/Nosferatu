using UnityEngine;
using System.Collections;

public class LavaSurface : MonoBehaviour {
    private float _loopTime = 2.0f;
    private Renderer _renderer;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float r = Mathf.Sin((Time.time / _loopTime) * (2 * Mathf.PI)) * 0.5f + 0.25f;
        float g = Mathf.Sin((Time.time / _loopTime + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float b = Mathf.Sin((Time.time / _loopTime + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float correction = 1 / (r + g + b);
        r *= correction;
        g *= correction;
        b *= correction;
        _renderer.material.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
    }
}
