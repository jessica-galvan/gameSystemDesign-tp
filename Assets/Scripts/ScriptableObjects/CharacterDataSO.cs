using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "TP/CharacterData", order = 4)]
public class CharacterDataSO : ScriptableObject
{
    [Header("Health")]
    public int maxLife = 100;
    public float radius = 2f;
    public int damage = 1;
    public float experience = 1f;
    public float takeDamageCooldown = 0f;
    public float takeDamageRecolorTime = 0.2f;
    public Color takeDamageColor = Color.red;

    [Header("Movement")]
    public bool canBeKockedBack = true;
    public float movementSpeed = 2f;

    [Header("IA Stats")]
    [Tooltip("Prediction time affects how far away will predict, the lower it is, the lower it will take to turn, the higher it goes, the faster ir turn or completly skip it")]
    public float predictionTime = 0.0000f;
    public float minDistanceFromPlayer = 0.5f;
}
