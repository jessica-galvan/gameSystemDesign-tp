using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatsSo", menuName = "TP/CharacterBaseStats", order = 2)]
public class CharacterBaseStatsSO : ScriptableObject
{
    [Header("Health")]
    public int maxLife = 100;
    public float radius = 2f;
    public float damage = 1f;
    public float experience = 1f;

    [Header("Movement")]
    public float movementSpeed = 2f;
    public float turnSpeed = 5f;
    public float knockbackRecovery = 0f;

    [Header("IA Stats")]
    public float predictionTime = 1f;
    public float minDistanceFromPlayer = 0.5f;
}
