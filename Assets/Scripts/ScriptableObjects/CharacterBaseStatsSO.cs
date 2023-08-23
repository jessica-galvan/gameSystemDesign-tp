using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseStatsSO : ScriptableObject
{
    [Header("Health")]
    public int maxLife;

    [Header("Movement")]
    public float movementSpeed;
}
