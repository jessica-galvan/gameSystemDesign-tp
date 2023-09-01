using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleBar : MonoBehaviour
{
    public Image imgBg;
    public Image imgFill;
    public TMP_Text txtTitle;

    public void SetValue(float amount)
    {
        imgFill.fillAmount = amount;
    }
}
