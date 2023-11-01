using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class BaseAbilityAction : ScriptableObject
{
    public abstract void Initialize();
    public abstract void Execute(PlayerModel playerModel);
    public abstract void PowerUp(float amountMultiplier, float attackMultiplier);
    public abstract void GetDescriptionForPowerUp(StringBuilder stringBuilder, PowerUpAbilitySO powerUp);
}
