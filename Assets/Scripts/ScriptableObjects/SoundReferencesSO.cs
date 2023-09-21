using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundReferences", menuName = "TP/General/SoundReferences", order = 5)]
public class SoundReferencesSO : ScriptableObject
{
    [Header("UI")]
    public AudioClip hoverButton;
    public AudioClip selectButton;
    public AudioClip pauseSound;
    public AudioClip mainMenu;
    public AudioClip openPopup;
    public AudioClip closePopup;

    [Header("Level")]
    public AudioClip spawnSound;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;
    public AudioClip playerShoot;
    public AudioClip negativeShootSound;

    [Header("Extras")]
    public AudioClip GameOver;
    public AudioClip levelMusic;
}
