using System;
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

    public bool Initialized { get; private set; }
    public Button Button { get; private set; }

    public event Action OnSelected;

    public virtual void Initialize()
    {
        if (Initialized) return;
        Initialized = true;

        Button = GetComponent<Button>();
        Deselect();
    }

    public virtual void Select()
    {
        if(GameManager.HasInstance)
            GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.hoverButtonSound);

        if (selectedMarker != null)
            selectedMarker.SetActive(true);

        OnSelected?.Invoke();
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

    private void RemoveAllListeners()
    {
        if (Button == null) return;
        Button.onClick.RemoveAllListeners();
    }
}

