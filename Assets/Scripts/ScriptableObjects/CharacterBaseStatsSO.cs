using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatsSo", menuName = "TP/CharacterBaseStats", order = 2)]
public class CharacterBaseStatsSO : ScriptableObject
{
    [Header("Health")]
    public int maxLife = 100;

    [Header("Movement")]
    public float movementSpeed = 2f;

    public float shootingCooldown = 2f;
}
