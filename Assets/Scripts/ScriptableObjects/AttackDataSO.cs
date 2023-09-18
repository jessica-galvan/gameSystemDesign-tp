using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "TP/AttackData", order = 3)]
public class AttackDataSO : ScriptableObject
{
    public int damage = 0;
    public bool applyKnockback = true;
    public float force = 0f;
}
