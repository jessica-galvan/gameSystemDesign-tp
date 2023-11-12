using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundReferences", menuName = "TP/General/SoundReferences", order = 5)]
public class SoundReferencesSO : ScriptableObject
{
    public bool canPlaySounds = true;

    [Header("UI")]
    public AudioClip hoverButtonSound;
    public AudioClip selectButtonSound;
    public AudioClip pauseSound;
    public AudioClip openPopup;
    public AudioClip closePopup;
    public AudioClip abilitySelectedSound;
    public AudioClip powerUpSelectedSound;

    [Header("Level")]
    public AudioClip playerTakeDamageSound;
    public AudioClip playerDeathSound;
    public AudioClip enemyTakeDamageSound;
    public AudioClip enemyDeathSound;
    public AudioClip playerShootSound;
    public AudioClip negativeShootSound;
    public AudioClip manaPickUpSound;
    public AudioClip manaPopSound;

    [Header("Extras")]
    public AudioClip levelUpSound;
    public AudioClip GameOverMusic;
    public AudioClip levelMusic;
    public AudioClip mainMenuMusic;
}
