using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "TP/SkillData", order = 2)]
public class AbilityDataSO : ScriptableObject
{
    [Header("Basic info")]
    public string title = "TITLE";
    public Sprite icon;
    [TextArea] public string description = "Add a description here";
    public float cooldown = 0.5f;
    public int manaCost = 5;

    //[Header("Movement")]
    //public bool locksMovement = false;
    //public bool locksFacing = false;
    //public float movementCooldown = 0f;

    [Header("Special")]
    [Tooltip("Heals or takes damage, depending if it's positive or negative")]
    public int health = 0;

    [Header("Projectile")]
    public bool usesProjectiles = false;
    public ProjectileController projectilePrefab;
    public int amount = 0;
    public float radiousFromPlayer = 0f;
}
