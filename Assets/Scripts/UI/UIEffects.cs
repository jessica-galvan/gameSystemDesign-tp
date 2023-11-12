using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffects : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [Header("LevelUP Effects")]
    [SerializeField] private ParticleSystem[] levelUpEffectsStopInstantly;
    [SerializeField] private ParticleSystem[] levelUpEffectStopsDelay;

    [ReadOnly, SerializeField] private Camera uiCamera;

    public void Initialize()
    {
        uiCamera = GameManager.Instance.cameraController.uiCamera;
        canvas.worldCamera = uiCamera;
    }

    public void SetLevelUpEffects(bool enabled)
    {
        if (enabled)
        {
            for (int i = 0; i < levelUpEffectStopsDelay.Length; i++)
                levelUpEffectStopsDelay[i].Play();

            for (int i = 0; i < levelUpEffectsStopInstantly.Length; i++)
                levelUpEffectsStopInstantly[i].Play();
        }
        else
        {
            for (int i = 0; i < levelUpEffectStopsDelay.Length; i++)
                levelUpEffectStopsDelay[i].Stop(true, ParticleSystemStopBehavior.StopEmitting);

            for (int i = 0; i < levelUpEffectsStopInstantly.Length; i++)
                levelUpEffectsStopInstantly[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
