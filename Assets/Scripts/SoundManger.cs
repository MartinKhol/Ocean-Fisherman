using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Audio class for playing sound effects and music

public class SoundManger : MonoBehaviour
{
    private static SoundManger _instance;
    public static SoundManger Instance { get { return _instance; } }

    public AudioMixer audioMixer;
    [Space]
    public AudioClip aboveWater;
    public AudioClip belowWater;
    public AudioSource ambientAudioSource;
    [Space]
    public AudioClip diveClip;
    public AudioClip riseClip;
    public AudioClip catchFishClip;
    public AudioClip damagedClip;
    public AudioClip cashInClip;
    public AudioClip lowOxygenClip;
    public AudioClip dashClip;
    public AudioSource effectsSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    public void AboveWater()
    {
        ambientAudioSource.clip = aboveWater;
        ambientAudioSource.Play();
    }

    public void BelowWater()
    {
        ambientAudioSource.clip = belowWater;
        ambientAudioSource.Play();
    }

    public void PlayDive()
    {
        effectsSource.PlayOneShot(diveClip);
    }

    public void PlayRise()
    {
        effectsSource.PlayOneShot(riseClip);
    }
    public void PlayCatchFish()
    {
        effectsSource.PlayOneShot(catchFishClip);
    }
    public void PlayDamaged()
    {
        effectsSource.PlayOneShot(damagedClip);
    }
    public void PlayCashIn()
    {        
        effectsSource.PlayOneShot(cashInClip);
    }
    public void PlayOxygenWarning()
    {
        effectsSource.PlayOneShot(lowOxygenClip);
    }
    public void PlayDash()
    {
        effectsSource.PlayOneShot(dashClip);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
    public void SetAmbientVolume(float volume)
    {
        audioMixer.SetFloat("Ambient", volume);
    }
}
