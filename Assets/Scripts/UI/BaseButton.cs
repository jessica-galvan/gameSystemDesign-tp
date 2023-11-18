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

    [SerializeField, ReadOnly] private bool shown, shownMarkers, mouseOver;
 
    public bool Initialized { get; private set; }
    public Button Button { get; private set; }

    public event Action OnSelected;

    public virtual void Initialize()
    {
        if (Initialized) return;
        Initialized = true;

        Button = GetComponent<Button>();

        ShowMarkers(false, force: true);
        Deselect();
    }

    public virtual void Select()
    {
        if(GameManager.HasInstance)
            GameManager.Instance.audioManager.PlaySFXSound(GameManager.Instance.soundReferences.hoverButtonSound);

        ShowMarkers(true);

        OnSelected?.Invoke();
    }

    public virtual void Deselect()
    {
        ShowMarkers(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
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

    public void Show(bool show, bool force = false)
    {
        if (show == shown && !force) return;
        shown = show;
        gameObject.SetActive(shown);
    }

    public void ShowMarkers(bool show, bool force = false)
    {
        if (selectedMarker == null) return;
        if (show == shownMarkers && !force) return;
        shownMarkers = show;
        selectedMarker.SetActive(shownMarkers);
    }

    private void RemoveAllListeners()
    {
        if (Button == null) return;
        Button.onClick.RemoveAllListeners();
    }
}

