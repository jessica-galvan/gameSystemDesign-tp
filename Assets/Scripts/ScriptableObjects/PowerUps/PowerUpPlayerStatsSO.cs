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
    [TextArea, SerializeField] private string description = "Add a description here";
    [SerializeField] private Stats statsToModify;
    [SerializeField] private float add = 0;
    [SerializeField] private float multiplier = 0;

    public override void Initialize()
    {
        Debug.Assert((multiplier > 0 && add == 0) || (multiplier == 0 && add > 0), $"Either multiply or add, do not do both. PowerUp: {name}");

        if (add > 0)
            description = string.Format(description, add);
        else if (multiplier > 0)
            description = string.Format(description, multiplier * 10);

        //if (add > 0)
        //    description += $"- {statsToModify}: +{add}";
        //else if (multiplier > 0)
        //    description += $"- {statsToModify}: x{multiplier}";
    }

    public override void Execute()
    {
        base.Execute();

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
                var newSpeed = NewValue(GameManager.Instance.Player.Speed);
                GameManager.Instance.Player.SetNewSpeed(newSpeed);
                break;
            case Stats.ExperienceMultiplier:
                var newExperience = NewValue(GameManager.Instance.experienceSystem.ExperienceMultiplier);
                GameManager.Instance.experienceSystem.SetNexExperienceMultiplier(newExperience);
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
            return oldValue + (oldValue * multiplier);

        return oldValue;
    }

    private int NewValue(int oldValue)
    {
        if (add != 0)
            return (int) (oldValue + add);
        else if (multiplier != 0)
            return oldValue + (int) (oldValue * multiplier);

        return oldValue;
    }

    public override string GetDescription()
    {
        return description;
    }
}
