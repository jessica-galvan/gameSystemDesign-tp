using System;
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
    [SerializeField] private GameObject hasNoManaContainer;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgTimer;

    [ReadOnly, SerializeField] private AbilityDataSO abilityData;
    [ReadOnly, SerializeField] private float currentT;
    [ReadOnly, SerializeField] private bool hasMana, isInCooldown;

    public AbilityDataSO AbilityData => abilityData;
    public bool IsUnlocked { get; private set; }

    public void Initialize()
    {
        SetAbilityAsEnable(false);

        isInCooldown = false;
        imgTimer.enabled = isInCooldown;
        txtCooldown.enabled = isInCooldown;

        hasMana = true;
        hasNoManaContainer.gameObject.SetActive(hasMana);
    }

    public void Refresh()
    {
        if (!IsUnlocked) return;
        CheckStatus();

        if (abilityData.IsInCooldown)
            UpdateTimer(abilityData.CurrentTimeLeft);
    }

    private void CheckStatus()
    {
        var hasMana = abilityData.HasMana(GameManager.Instance.manaSystem.currentMana);
        if (this.hasMana != hasMana)
        {
            this.hasMana = hasMana;
            hasNoManaContainer.gameObject.SetActive(!hasMana);
        }

        if(isInCooldown != abilityData.IsInCooldown)
        {
            isInCooldown = abilityData.IsInCooldown;
            imgTimer.enabled = isInCooldown;
            txtCooldown.enabled = isInCooldown;
        }
    }

    private void SetAbilityAsEnable(bool enabled)
    {
        IsUnlocked = enabled;

        lockedStatusContainer.SetActive(!enabled);
        txtManaCost.enabled = enabled;
        txtKeyboard.enabled = enabled;
        imgIcon.enabled = enabled;
    }

    public void SetAbility(AbilityDataSO abilityData, int index)
    {
        this.abilityData = abilityData;

        SetAbilityAsEnable(true);

        txtManaCost.SetText(abilityData.manaCost.ToString());
        txtKeyboard.SetText(index.ToString());

        imgIcon.sprite = abilityData.icon;
    }

    private void UpdateTimer(float currentTime)
    {
        txtCooldown.SetText(TimeSpan.FromSeconds(currentTime).ToString("%s"));

        currentT = Mathf.InverseLerp(0, abilityData.cooldown, currentTime);
        imgTimer.fillAmount = currentT;
    }
}
