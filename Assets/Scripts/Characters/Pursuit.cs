using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pursuit
{
    private const float EPSILON = 0.1f;
    private PlayerModel target;

    public Pursuit()
    {
        target = GameManager.Instance.Player;
    }

    public Vector2 GetDir(EnemyModel entity)
    {
        //Movimiento Rectilineo Uniforme = Posicion Actual + Direccion * Velocidad * Tiempo
        //Se hizo un clamp para evitar que si el enemigo cambia el foward, no vaya en contra del objetivo. 

        if (!target.Alive) return Vector2.zero;

        float distance = Vector2.Distance(entity.transform.position, target.transform.position) - EPSILON;
        var speed = Mathf.Clamp(target.BaseStats.movementSpeed * entity.BaseStats.predictionTime, -distance, distance);
        Vector2 targetPoint = (Vector2) target.transform.position + target.Direction * speed;
        Vector2 dir = targetPoint - (Vector2) entity.transform.position;
        return dir.normalized;
    }
}
