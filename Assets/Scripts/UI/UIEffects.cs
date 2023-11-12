using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffects : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem levelUpEffect;

    [ReadOnly, SerializeField] private Camera uiCamera;

    public void Initialize()
    {
        uiCamera = GameManager.Instance.cameraController.uiCamera;
        canvas.worldCamera = uiCamera;
    }

    public void SetLevelUpEffects(bool enabled)
    {
        if (enabled)
            levelUpEffect.Play();
        else
            levelUpEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        //levelUpEffect.gameObject.SetActive(enabled);
    }
}
