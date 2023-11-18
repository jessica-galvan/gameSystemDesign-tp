using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiateAction", menuName = "TP/AbilityData/InstantiateAction", order = 2)]
public class InstantiateAction : BaseAbilityAction
{
    public GameObject objectToInstantiate;

    public string itemName = "";
    public bool useMouseDirection = false; //USE ONLY IF IS PROJECTILE
    public int baseAmount = 0;
    public float timeAlive = 5f;
    public float minDistanteFromPlayer = 0f;
    public float maxDistanteFromPlayer = 5f;
    public Vector2[] directions;

    [SerializeField] private bool isProjectile, canDamage;

    [NonSerialized] private ProjectileController projectile;
    [NonSerialized] private IDamage damageObject;
    [field: ReadOnly, NonSerialized] public int Amount { get; set; }

    public override void Initialize()
    {
        Amount = baseAmount;

        projectile = objectToInstantiate.GetComponent<ProjectileController>();
        isProjectile = projectile != null;

        damageObject = objectToInstantiate.GetComponent<IDamage>();
        canDamage = damageObject != null;
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
        //TODO DO THIS BETTER
        var aux = GameObject.Instantiate(objectToInstantiate);
        aux.transform.position = playerModel.transform.position;

        if (aux.TryGetComponent<IInstantiableAction>(out IInstantiableAction item))
            item.Initialize(timeAlive);
    }

    public override void PowerUp(float amountMultiplier, float attackMultiplier)
    {
        Amount = Mathf.RoundToInt(Amount * amountMultiplier);

        if (attackMultiplier > 0 && canDamage)
            damageObject.AttackData.PowerUp(attackMultiplier);
    }

    public override void GetDescriptionForPowerUp(StringBuilder stringBuilder, PowerUpAbilitySO powerUp)
    {
        if(powerUp.AmountMultiplier > 0)
            stringBuilder.AppendLine($"- Gains an {powerUp.AmountMultiplier * 100}% of extra {itemName}");

        if(powerUp.AttackMultiplier > 0)
            stringBuilder.AppendLine($"- Gains an {powerUp.AttackMultiplier * 100}% of extra damage");
    }
}
