using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpButton : BaseButton
{
    [Header("References")]
    [SerializeField] private Image imgIcon;
    [SerializeField] private TMP_Text txtDescription;
    [SerializeField] private Image imgBorder;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private GameObject powerUpMarker;

    [Header("Info")]
    [SerializeField, ReadOnly] private bool isPowerUp = false;

    private Color originalBorderColor;

    public ISelectableOption CurrentOption { get; private set; }

    public override void Initialize()
    {
        originalBorderColor = imgBorder.color;
        base.Initialize();
    }

    public void SetSelectableOption(ISelectableOption option)
    {
        CurrentOption = option;

        txtTitle.SetText(CurrentOption.Title);
        txtDescription.SetText(CurrentOption.GetDescription());

        if(imgIcon != null)
            imgIcon.sprite = CurrentOption.Icon;

        isPowerUp = option.IsPowerUp();
        powerUpMarker.SetActive(isPowerUp);
    }

    public override void Select()
    {
        base.Select();
        if (imgBorder != null)
            imgBorder.color = selectedColor;
    }

    public override void Deselect()
    {
        base.Deselect();
        if (imgBorder != null)
            imgBorder.color = originalBorderColor;
    }
}
