using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "TP/ProjectileData", order = 2)]
public class ProjectileDataSO : ScriptableObject
{
    public ProjectileType type;
    public float speed = 5f;
    public float cooldown = 2f;
    public float totalTimeAlive = 5f;
}
