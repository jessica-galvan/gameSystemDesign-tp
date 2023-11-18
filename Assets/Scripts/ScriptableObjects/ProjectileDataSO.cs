using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "TP/Damage/ProjectileData", order = 2)]
public class ProjectileDataSO : ScriptableObject
{
    public ParticleSystem bulletDestructionVFX;
    public ProjectileType type;
    public float startingSpeed = 4f;
    public float speed = 5f;
    public float cooldown = 2f;
    public float totalTimeAlive = 5f;

    public float Cooldown { get; private set; }
    public float Speed { get; private set; }
    public float StartingSpeed { get; private set; }
    public float TimeAlive { get; private set; }

    public void Initialize()
    {
        Speed = speed;
        TimeAlive = totalTimeAlive;
        StartingSpeed = startingSpeed;
        Cooldown = cooldown;
    }

    public void PowerUpSpeed(float multiplier)
    {
        Speed += (Speed * multiplier);
        StartingSpeed += (StartingSpeed * multiplier);
    }

    public void PowerUpTimeAlive(float multiplier)
    {
        TimeAlive += (TimeAlive * multiplier);
    }

    public void ReduceCooldown(float multiplier)
    {
        Cooldown -=  (Cooldown * multiplier);
    }
}
