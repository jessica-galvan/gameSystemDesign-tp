using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : MonoBehaviour, ICollectable, IUpdate, IPoolable
{
    public SpriteRenderer spriteRenderer;
    public int manaAmount = 5;
    public float lifeTimer = 10f;
    public float startFadingTime = 0.5f;
    public float minFadingAmount = 0.3f;

    private float currentTimer = 0f;
    private float currentT = 1f;
    private Color fadingColor = Color.white;

    public void Initialize() { }

    public void Restart()
    {
        gameObject.SetActive(true);
        currentTimer = lifeTimer;
        currentT = 1f;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }

    public void PickUp()
    {
        GameManager.Instance.manaSystem.AddMana(manaAmount);
        Die();
    }

    public void Refresh(float deltaTime)
    {
        if (GameManager.Instance.Pause) return;
        currentTimer -= deltaTime;

        currentT = Mathf.InverseLerp(0, lifeTimer, currentTimer);

        if (currentT < startFadingTime)
        {
            fadingColor = spriteRenderer.color;
            fadingColor.a = minFadingAmount + currentT;
            spriteRenderer.color = fadingColor;
        }

        if (currentTimer <= 0)
            Die(destroyed: true);
    }

    private void Die(bool destroyed = false)
    {
        if (destroyed)
        {
            var vfx = GameManager.Instance.poolManager.GetParticle(ParticleEffect.ParticleType.ManaDestruction);
            vfx.Set(transform);
        }

        var sound = destroyed ? GameManager.Instance.soundReferences.manaPopSound : GameManager.Instance.soundReferences.manaPickUpSound;
        GameManager.Instance.audioManager.PlaySFXSound(sound);

        GameManager.Instance.poolManager.ReturnManaDrop(this);
    }

    public void ReturnToPool()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        gameObject.SetActive(false);
    }
}
