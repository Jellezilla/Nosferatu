using UnityEngine;
using System.Collections;


public class TombstoneAudio : MonoBehaviour {
    [SerializeField]
    private AudioClip m_TombstoneImpact;
    [SerializeField]
    [Range(0, 1)]
    private float m_volume;
    private AudioSource m_Tombstone;
    private Tombstone m_Tomb;
    private bool m_played;

    // Use this for initialization
    void Start () {

        m_Tomb = GetComponent<Tombstone>();
        InitTombstoneAudio(m_TombstoneImpact);
	
	}

    void Update()
    {
        PlayBreak();
    }

    void InitTombstoneAudio(AudioClip clip)
    {
       m_Tombstone = gameObject.AddComponent<AudioSource>();
       m_Tombstone.clip = clip;
       m_Tombstone.loop = false;
       m_Tombstone.playOnAwake = false;
       m_Tombstone.minDistance = 5;
       m_Tombstone.maxDistance = 500;
       m_Tombstone.dopplerLevel = 0;
       m_Tombstone.volume = m_volume;
    }

    void PlayBreak()
    {
        if (m_played && !m_Tomb.IsBroken)
        {
            Debug.Log("false");
            m_played = false;
        }
        else if (m_Tomb.IsBroken && !m_played)
        {
            Debug.Log("true");
            m_played = true;
            m_Tombstone.Play();
        }
    }

}
