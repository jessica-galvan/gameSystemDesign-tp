using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatsSo", menuName = "TP/CharacterBaseStats", order = 2)]
public class CharacterBaseStatsSO : ScriptableObject
{
    [Header("Health")]
    public int maxLife = 100;
    public float radius = 2f;

    [Header("Movement")]
    public float movementSpeed = 2f;
    public float turnSpeed = 5f;
}
