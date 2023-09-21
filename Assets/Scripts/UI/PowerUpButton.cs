using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpButton : BaseButton
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text txtDescription;

    public ISelectableOption CurrentOption { get; private set; }

    public void SetSelectableOption(ISelectableOption option)
    {
        CurrentOption = option;

        txtTitle.SetText(CurrentOption.Title);
        txtDescription.SetText(CurrentOption.Description);

        if(image != null)
            image.sprite = CurrentOption.Icon;
    }
}
