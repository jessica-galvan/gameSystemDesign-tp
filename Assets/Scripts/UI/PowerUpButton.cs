using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpButton : BaseButton
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private TMP_Text txtDescription;
    [SerializeField] private Image imgBorder;
    [SerializeField] private Color selectedColor = Color.white;

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
        txtDescription.SetText(CurrentOption.Description);

        if(imgIcon != null)
            imgIcon.sprite = CurrentOption.Icon;
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
