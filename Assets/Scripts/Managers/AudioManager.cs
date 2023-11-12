using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool canPlaySounds = true;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    public void PlayMusic(AudioClip audio)
    {
        if (!canPlaySounds || audio == null) return;

        if (musicAudioSource.isPlaying)
            musicAudioSource.Stop();

        musicAudioSource.PlayOneShot(audio);
    }


    public void PlaySFXSound(AudioClip audio)
    {
        if (!canPlaySounds ||  audio == null) return;
        sfxAudioSource.PlayOneShot(audio);
    }

}
