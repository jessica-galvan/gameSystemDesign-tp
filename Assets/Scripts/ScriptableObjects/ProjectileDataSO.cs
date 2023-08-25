using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "TP/ProjectileData", order = 2)]
public class ProjectileDataSO : ScriptableObject
{
    public float speed = 5f;
    public float cooldown = 2f;
    public int baseDamage = 1;
    public float totalTimeAlive = 5f;
}
