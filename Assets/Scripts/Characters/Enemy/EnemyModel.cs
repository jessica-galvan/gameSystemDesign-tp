using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : BaseCharacterModel
{
    public Pursuit pursuit;

    public override void Initialize()
    {
        base.Initialize();
        pursuit = new Pursuit();
    }

    public bool CanMove(Vector2 playerPos)
    {
        //Although radious is easier, the game is by cells, so box is better. 
        float distance = (playerPos - (Vector2) transform.position).magnitude;
        return distance > baseStats.minDistanceFromPlayer;
    }
}
