using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundReferencesSO soundReferences;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    public void PlayMusic(AudioClip audio)
    {
        if (audio == null) return;

        if (musicAudioSource.isPlaying)
            musicAudioSource.Stop();

        musicAudioSource.PlayOneShot(audio);
    }


    public void PlaySFXSound(AudioClip audio)
    {
        if (audio == null) return;
        sfxAudioSource.PlayOneShot(audio);
    }

}
