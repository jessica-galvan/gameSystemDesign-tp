using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.PreUpdate;

public class SimpleBar : MonoBehaviour, IUpdate
{
    public Image imgBg;
    public Image imgFill;
    public TMP_Text txtTitle;

    [Header("Color Change")]
    public bool colorGradient = false;
    public Color fullColor =  Color.white;
    public Color emptyColor = Color.black;

    [Header("Empty VFX")]
    public bool emptyEffect = false;
    public Image imgEmpty;
    public float emptySpeed = 2f;
    private bool canUpdate;
    private float currentAmount = 1f, newAmount;

    public void Initialize()
    {
        imgEmpty.enabled = emptyEffect;
    }

    public void SetValue(float amount, bool skipEffect = false)
    {
        imgFill.fillAmount = amount;
        newAmount = amount;

        if (colorGradient)
            imgFill.color = Color.Lerp(emptyColor, fullColor, amount);

        if (emptyEffect )
        {
            if (!skipEffect && currentAmount > newAmount)
            {
                canUpdate = true;
                imgEmpty.fillAmount = currentAmount;
                GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
            }
            else
            {
                currentAmount = newAmount;
                imgEmpty.fillAmount = currentAmount;
            }
        }
    }

    private void EmptyEffect()
    {
        canUpdate = false;
        GameManager.Instance.updateManager.uiCustomUpdate.Remove(this);
    }

    public void Refresh(float deltaTime)
    {
        if (emptyEffect && canUpdate)
        {
            currentAmount -= (deltaTime * emptySpeed);
            imgEmpty.fillAmount = currentAmount;
            if (currentAmount <= newAmount)
                EmptyEffect();
        }
    }
}
