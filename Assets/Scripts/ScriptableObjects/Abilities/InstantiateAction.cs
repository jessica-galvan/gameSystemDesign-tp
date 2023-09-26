using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiateAction", menuName = "TP/AbilityData/InstantiateAction", order = 2)]
public class InstantiateAction : BaseAbilityAction
{
    public GameObject objectToInstantiate;

    public bool isProjectile = false;
    public bool useMouseDirection = false; //ONLY IF IS PROJECTILE
    public int baseAmount = 0;
    public float minDistanteFromPlayer = 0f;
    public float maxDistanteFromPlayer = 5f;
    public Vector2[] directions;

    [NonSerialized] private ProjectileController projectile;
    [field: ReadOnly, NonSerialized] public float Amount { get; set; }

    public override void Initialize()
    {
        Amount = baseAmount;

        projectile = objectToInstantiate.GetComponent<ProjectileController>();
        isProjectile = projectile != null;
    }

    public override void Execute(PlayerModel playerModel)
    {
        if (isProjectile)
            SetProjectile(playerModel);
        else
            SetObject(playerModel);

        Debug.Log($"Ability {name} was executed");
    }

    private void SetProjectile(PlayerModel playerModel)
    {
        if (useMouseDirection)
            playerModel.Shoot(GameManager.Instance.cameraController.MouseWorldPos(), projectile.data);
        else
            Debug.Log("nice shot");

        //TODO do logic so that they spawn from player in a radius going to the oustide
    }

    private void SetObject(PlayerModel playerModel)
    {
        var aux = GameObject.Instantiate(objectToInstantiate);
        aux.transform.position = playerModel.transform.position;
        //TODO DO THIS BETTER
    }
}
