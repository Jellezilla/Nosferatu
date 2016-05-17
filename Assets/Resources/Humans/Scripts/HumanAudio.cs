using UnityEngine;
using System.Collections;

public class HumanAudio : MonoBehaviour {

    [SerializeField]
    [Range(0, 1)]
    private float m_volume = 0;
    [SerializeField]
    private AudioClip m_HumanSplat;
    private AudioSource m_SplatAudio;
	// Use this for initialization
	void Awake ()
    {
        m_SplatAudio = SetupHumanAudioSource(m_HumanSplat);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.player)
        {
            m_SplatAudio.Play();
        }
    }


    private AudioSource SetupHumanAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = m_volume;   
        source.loop = false;
        source.playOnAwake = false;
        source.minDistance = 5;
        source.maxDistance = 30;
        source.dopplerLevel = 0;
        return source;
    }

}
