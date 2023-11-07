using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : MonoBehaviour, ICollectable, IUpdate, IPoolable
{
    public int manaAmount = 5;
    public float lifeTimer = 10f;

    private float currentTimer = 0f;

    public void Initialize() { }

    public void Restart()
    {
        gameObject.SetActive(true);
        currentTimer = lifeTimer;
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

        GameManager.Instance.poolManager.ReturnManaDrop(this);
    }

    public void ReturnToPool()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        gameObject.SetActive(false);
    }
}
