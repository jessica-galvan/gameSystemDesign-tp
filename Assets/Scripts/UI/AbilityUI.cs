using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private TMP_Text txtManaCost;
    [SerializeField] private TMP_Text txtKeyboard;
    [SerializeField] private TMP_Text txtCooldown;
    [SerializeField] private GameObject lockedStatusContainer;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgTimer;

    [ReadOnly, SerializeField] private AbilityDataSO abilityData;
    [ReadOnly, SerializeField] private float currentT;

    public AbilityDataSO AbilityData => abilityData;
    public bool IsUnlocked { get; private set; }

    public void Initialize()
    {
        SetAbilityAsEnable(false);
    }

    private void SetAbilityAsEnable(bool enabled)
    {
        IsUnlocked = enabled;

        lockedStatusContainer.SetActive(!enabled);
        txtManaCost.enabled = enabled;
        txtKeyboard.enabled = enabled;
        imgIcon.enabled = enabled;
        imgTimer.enabled = enabled;

        if (txtCooldown != null)
            txtCooldown.enabled = enabled;
    }

    public void SetAbility(AbilityDataSO abilityData, int index)
    {
        this.abilityData = abilityData;

        SetAbilityAsEnable(true);

        txtManaCost.SetText(abilityData.manaCost.ToString());
        txtKeyboard.SetText(index.ToString());

        imgIcon.sprite = abilityData.icon;
    }

    public void UpdateTimer(float currentTime)
    {
        currentT = Mathf.InverseLerp(0, abilityData.cooldown, currentTime);
        imgTimer.fillAmount = currentT;
    }
}
