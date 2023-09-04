using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_Text txtTitle;
    public GameObject selectedMarker;
    public Button Button { get; private set; }

    public virtual void Initialize()
    {
        Button = GetComponent<Button>();
        Deselect();
    }

    public virtual void Select()
    {
        GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.hoverButton);

        if (selectedMarker != null)
            selectedMarker.SetActive(true);
    }

    public virtual void Deselect()
    {
        if (selectedMarker != null)
            selectedMarker.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }

    public virtual void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}

