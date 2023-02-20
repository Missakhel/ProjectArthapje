using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{ 
    kMusic,
    kAmbient,
    kSFX,
    kUI
}

public enum SFX
{ 
    kSwingOrbHit,
    kSwingEnemyHit,
    kEnemyNormalBounce,
    kEnemyDamageBounce,
    kOrbEnemyHit,
    kBumperBounce,
    kEnemyPlayerHit,
    kShieldHit,
    kPull,
    kCharge,
    kHold,
    kTotal
}

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    //public static AudioManager m_Instance;

    public List<AudioSource> m_audioSources;
    public float m_masterVolume = 1.0f;
    public AudioClip m_gameMusic;
    public AudioClip m_gameAmbient;
    public Vector2 m_pitchRange;
    public float m_minimumPitch;
    public float m_pitchValue = .15f;
    public AudioClip[] m_SFXArray = new AudioClip[(int)SFX.kTotal];

    //SFX
    /*public AudioClip m_swingOrbHit;
    public AudioClip m_swingEnemyHit;
    public AudioClip m_enemyNormalBounce;
    public AudioClip m_enemyDamageBounce;
    public AudioClip m_orbEnemyHit;
    public AudioClip m_bumperBounce;*/

    public List<float> m_volume;

    private void Awake()
    {
        /*if(m_Instance != null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    public void PlayAudioClip(SFX SFXIndex, float volume)
    {
        m_audioSources[(int)AudioType.kSFX].PlayOneShot(m_SFXArray[(int)SFXIndex], volume * m_volume[(int)AudioType.kSFX] * m_masterVolume);
    }

    public void PlayAudioClipStrengthPitch(SFX SFXIndex, float volume, float hitMagnitude)
    {
        if (m_SFXArray[(int)SFXIndex])
        {
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.pitch = m_minimumPitch + (m_pitchValue * hitMagnitude);
            tempSource.PlayOneShot(m_SFXArray[(int)SFXIndex], volume * m_volume[(int)AudioType.kSFX] * m_masterVolume);
            Destroy(tempSource, m_SFXArray[(int)SFXIndex].length / tempSource.pitch);
        }
    }

    public void PlayAudioClipRandomPitch(SFX SFXIndex, float volume)
    {
        if(m_SFXArray[(int)SFXIndex])
        {
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.pitch = Random.Range(m_pitchRange.x, m_pitchRange.y);
            tempSource.PlayOneShot(m_SFXArray[(int)SFXIndex], volume * m_volume[(int)AudioType.kSFX] * m_masterVolume);
            Destroy(tempSource, m_SFXArray[(int)SFXIndex].length / tempSource.pitch);
        }
    }

    public void SetPitch(AudioType audioType, float newPitch)
    {
        m_audioSources[(int)audioType].pitch = newPitch;
    }

    public void Stop(AudioType audioType)
    {
        m_audioSources[(int)audioType].Stop();
    }

    public void Unpause(AudioType audioType)
    {
        m_audioSources[(int)audioType].UnPause();
    }

    public void SetAudioClip(AudioType audioType, AudioClip audioClip, float volume, bool loop = false)
    {
        m_audioSources[(int)audioType].Stop();
        m_audioSources[(int)audioType].volume = volume * m_volume[(int)audioType] * m_masterVolume;
        m_audioSources[(int)audioType].clip = audioClip;
        m_audioSources[(int)audioType].loop = loop;
        m_audioSources[(int)audioType].Play();
    }

    public void PlaySFX(SFX sfx)
    {

    }

    public void InitiateGameMusic()
    {
        SetAudioClip(AudioType.kMusic, m_gameMusic, .75f, true);
        SetAudioClip(AudioType.kAmbient, m_gameAmbient, .75f, true);
    }
}
