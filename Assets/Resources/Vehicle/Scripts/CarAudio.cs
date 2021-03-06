using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(VehicleController))]
public class CarAudio : MonoBehaviour
{
    // This script reads some of the car's current properties and plays sounds accordingly.
    // The engine sound can be a simple single clip which is looped and pitched, or it
    // can be a crossfaded blend of four clips which represent the timbre of the engine
    // at different RPM and Throttle state.

    // the engine clips should all be a steady pitch, not rising or falling.

    // when using four channel engine crossfading, the four clips should be:
    // lowAccelClip : The engine at low revs, with throttle open (i.e. begining acceleration at very low speed)
    // highAccelClip : Thenengine at high revs, with throttle open (i.e. accelerating, but almost at max speed)
    // lowDecelClip : The engine at low revs, with throttle at minimum (i.e. idling or engine-braking at very low speed)
    // highDecelClip : Thenengine at high revs, with throttle at minimum (i.e. engine-braking at very high speed)

    // For proper crossfading, the clips pitches should all match, with an octave offset between low and high.


    public enum EngineAudioOptions // Options for the engine audio
    {
        Simple, // Simple style audio
        FourChannel // four Channel audio
    }
    [SerializeField]
    private AudioClip hookLaunchClip;
    [SerializeField]
    private AudioClip lavaSplashClip;
    [SerializeField]
    private AudioClip nitroClip;
    [SerializeField]
    private EngineAudioOptions engineSoundStyle = EngineAudioOptions.FourChannel;// Set the default audio options to be four channel
    [SerializeField]
    private AudioClip lowAccelClip;                                              // Audio clip for low acceleration
    [SerializeField]
    private AudioClip lowDecelClip;                                              // Audio clip for low deceleration
    [SerializeField]
    private AudioClip highAccelClip;                                             // Audio clip for high acceleration
    [SerializeField]
    private AudioClip highDecelClip;                                             // Audio clip for high deceleration
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float pitchMultiplier = 1f;                                          // Used for altering the pitch of audio clips
    [SerializeField]
    private float lowPitchMin = 1f;                                              // The lowest possible pitch for the low sounds
    [SerializeField]
    private float lowPitchMax = 6f;                                              // The highest possible pitch for the low sounds
    [SerializeField]
    private float highPitchMultiplier = 0.25f;                                   // Used for altering the pitch of high sounds
    [SerializeField]
    private float maxRolloffDistance = 500;                                      // The maximum distance where rollof starts to take place
    [SerializeField]
    private float dopplerLevel = 1;                                              // The amount of doppler effect used in the audio
    [SerializeField]
    private bool useDoppler = true;                                              // Toggle for using doppler

    private AudioSource m_LowAccel; // Source for the low acceleration sounds
    private AudioSource m_LowDecel; // Source for the low deceleration sounds
    private AudioSource m_HighAccel; // Source for the high acceleration sounds
    private AudioSource m_HighDecel; // Source for the high deceleration sounds
    private AudioSource m_HookLaunch; // Source for the hook launcher sounds
    private AudioSource m_LavaSplash;
    private AudioSource m_Nitro;
    private bool m_StartedSound; // flag for knowing if we have started sounds
    private VehicleController m_CarController; // Reference to the car we are controlling
    private VehicleTurret m_CarTurret; // Reference to the turret we are firing
    private VehicleResources m_CarResources;
    private VehicleNitro m_CarNitro;
    private bool m_Shoot;
    private bool m_Splash;
    private bool m_NitroOn;
    private void StartSound()
    {
        // get the carcontroller ( this will not be null as we have require component)
        m_CarController = GetComponent<VehicleController>();
        m_CarTurret = GetComponent<VehicleTurret>();
        m_CarResources = GetComponent<VehicleResources>();
        m_CarNitro = GetComponent<VehicleNitro>();

        // setup the simple audio source


        // if we have four channel audio setup the four audio sources
        if (engineSoundStyle == EngineAudioOptions.FourChannel)
        {
            m_HighAccel = SetUpAudioSource(highAccelClip, true, true, true, true);
            m_LowAccel = SetUpAudioSource(lowAccelClip, true, true, true, true);
            m_LowDecel = SetUpAudioSource(lowDecelClip, true, true, true, true);
            m_HighDecel = SetUpAudioSource(highDecelClip, true, true, true, true);
            m_HookLaunch = SetUpAudioSource(hookLaunchClip, false, false, false, false);
            m_LavaSplash = SetUpAudioSource(lavaSplashClip, false, false, false, false);
            m_Nitro = SetUpAudioSource(nitroClip, false, false, false, false);
        }
        else
        {
            m_HighAccel = SetUpAudioSource(highAccelClip, true, true, true, true);
            m_HookLaunch = SetUpAudioSource(hookLaunchClip, false, false, false, false);
            m_LavaSplash = SetUpAudioSource(lavaSplashClip, false, false, false, false);
            m_Nitro = SetUpAudioSource(nitroClip, false, false, false, false);
        }

        // flag that we have started the sounds playing
        m_StartedSound = true;
    }


    private void StopSound()
    {
        //Destroy all audio sources on this object:
        foreach (var source in GetComponents<AudioSource>())
        {
            Destroy(source);
        }

        m_StartedSound = false;
    }


    // Update is called once per frame
    private void Update()
    {
        if (m_StartedSound && !m_NitroOn && m_CarNitro.UsingNitro)
        {
            m_Nitro.Play();
            m_NitroOn = true;

        }

        if (m_StartedSound && m_NitroOn && !m_CarNitro.UsingNitro)
        {
            m_NitroOn = false;
        }

        if (m_StartedSound && !m_Shoot && !m_CarTurret.isRetracted)
        {
            m_HookLaunch.Play();
            m_Shoot = true;
        }

        if (m_StartedSound && !m_Splash && m_CarResources.InLava)
        {
            m_LavaSplash.Play();
            m_Splash = true;
        }

        if (m_StartedSound && m_CarTurret.isRetracted)
        {
            m_Shoot = false;
        }

        if (m_StartedSound && m_Splash && !m_CarResources.InLava)
        {
            m_Splash = false;
        }
        // get the distance to main camera
        float camDist = (Camera.main.transform.position - transform.position).sqrMagnitude;

        // stop sound if the object is beyond the maximum roll off distance
        if (m_StartedSound && camDist > maxRolloffDistance * maxRolloffDistance)
        {
            StopSound();
        }

        // start the sound if not playing and it is nearer than the maximum distance
        if (!m_StartedSound && camDist < maxRolloffDistance * maxRolloffDistance)
        {
            StartSound();
        }

        if (m_StartedSound)
        {
            // The pitch is interpolated between the min and max values, according to the car's revs.
            float pitch = ULerp(lowPitchMin, lowPitchMax, m_CarController.Revs);

            // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
            pitch = Mathf.Min(lowPitchMax, pitch);

            if (engineSoundStyle == EngineAudioOptions.Simple)
            {
                // for 1 channel engine sound, it's oh so simple:
                m_HighAccel.pitch = pitch * pitchMultiplier * highPitchMultiplier;
                m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                m_HighAccel.volume = 1;
                m_HookLaunch.volume = 0.75f;
                m_Nitro.volume = 0.75f;
                m_LavaSplash.volume = 0.5f;
            }
            else
            {
                // for 4 channel engine sound, it's a little more complex:

                // adjust the pitches based on the multipliers
                m_LowAccel.pitch = pitch * pitchMultiplier;
                m_LowDecel.pitch = pitch * pitchMultiplier;
                m_HighAccel.pitch = pitch * highPitchMultiplier * pitchMultiplier;
                m_HighDecel.pitch = pitch * highPitchMultiplier * pitchMultiplier;

                // get values for fading the sounds based on the acceleration
                float accFade = Mathf.Abs(m_CarController.AccelInput);
                float decFade = 1 - accFade;

                // get the high fade value based on the cars revs
                float highFade = Mathf.InverseLerp(0.2f, 0.8f, m_CarController.Revs);
                float lowFade = 1 - highFade;

                // adjust the values to be more realistic
                highFade = 1 - ((1 - highFade) * (1 - highFade));
                lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
                accFade = 1 - ((1 - accFade) * (1 - accFade));
                decFade = 1 - ((1 - decFade) * (1 - decFade));

                // adjust the source volumes based on the fade values
                m_LowAccel.volume = lowFade * accFade;
                m_LowDecel.volume = lowFade * decFade;
                m_HighAccel.volume = highFade * accFade;
                m_HighDecel.volume = highFade * decFade;
                m_HookLaunch.volume = 0.75f;
                m_Nitro.volume = 0.75f;
                m_LavaSplash.volume = 0.5f;
                // adjust the doppler levels
                m_HighAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                m_LowAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                m_HighDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                m_LowDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            }
        }
    }


    // sets up and adds new audio source to the gane object
    private AudioSource SetUpAudioSource(AudioClip clip, bool looping, bool playing,bool randomStartPoint,bool playOnAwake)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = looping;
        source.playOnAwake = playOnAwake;

        if (randomStartPoint)
        {
            // start the clip from a random point
            source.time = Random.Range(0f, clip.length);
        }


        if (playing)
        {
            source.Play();
        }

        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        source.dopplerLevel = 0;
        return source;
    }


    // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }
}
