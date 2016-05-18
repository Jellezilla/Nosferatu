using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {

    [SerializeField]
    private AudioClip[] m_BackgroundMusic;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_volume;
    private AudioSource m_bgmSource;

	// Use this for initialization
	void Start () {

        InitSource();
        BGMPlayer(m_BackgroundMusic, m_bgmSource);

    }


    void InitSource()
    {
        m_bgmSource = gameObject.AddComponent<AudioSource>();
        m_bgmSource.loop = false;
        m_bgmSource.playOnAwake = true;
        m_bgmSource.minDistance = 5;
        m_bgmSource.maxDistance = 500;
        m_bgmSource.dopplerLevel = 0;
        m_bgmSource.volume = m_volume;
    }
	// Update is called once per frame
	void Update ()
    {
        ChaseCamera();
        BGMPlayer(m_BackgroundMusic, m_bgmSource);

    }

    void ChaseCamera()
    {
        if (Camera.main != null)
        {
            gameObject.transform.position = Camera.main.transform.position;
        }

    }

    private void BGMPlayer(AudioClip[] clips, AudioSource source)
    {

        if (!source.isPlaying)
        {
            int clipIndex = Random.Range(0, clips.Length);
            source.clip = clips[clipIndex];
            source.Play();
        }        
    }
}
