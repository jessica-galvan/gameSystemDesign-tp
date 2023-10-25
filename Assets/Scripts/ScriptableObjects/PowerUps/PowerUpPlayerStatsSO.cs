using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PUStat_", menuName = "TP/PowerUp/PowerUpStat", order = 0)]
public class PowerUpPlayerStatsSO : BasePowerUpSO
{
    public enum Stats
    {
        Mana,
        Life,
        Speed,
        ExperienceMultiplier
    }

    [SerializeField] private Stats statsToModify;
    [SerializeField] private float add = 0;
    [SerializeField] private float multiplier = 0;

    public override void Initialize()
    {
        newDescription = description;

        newDescription += "<br>";
        if (add > 0)
            description += $"- {statsToModify}: +{add}";
        else if (multiplier > 0)
            description += $"- {statsToModify}: x{multiplier}";
    }

    public override void Execute()
    {
        switch (statsToModify)
        {
            case Stats.Mana:
                var newMana = NewValue(GameManager.Instance.manaSystem.maxMana);
                GameManager.Instance.manaSystem.SetMaxMana(newMana);
                break;
            case Stats.Life:
                var newMaxLife = NewValue(GameManager.Instance.Player.LifeController.MaxLife);
                GameManager.Instance.Player.LifeController.SetNewMaxLife(newMaxLife);
                break;
            case Stats.Speed:
                break;
            case Stats.ExperienceMultiplier:
                break;
            default:
                break;
        }
    }

    private float NewValue(float oldValue)
    {
        if (add != 0)
            return oldValue + add;
        else if(multiplier != 0)
            return oldValue * multiplier;

        return oldValue;
    }

    private int NewValue(int oldValue)
    {
        if (add != 0)
            return (int) (oldValue + add);
        else if (multiplier != 0)
            return (int) (oldValue * multiplier);

        return oldValue;
    }

    public override string GetDescription()
    {

        return newDescription;
    }
}
