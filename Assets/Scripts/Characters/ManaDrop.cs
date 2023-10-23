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

    public void Refresh()
    {
        if (GameManager.Instance.Pause) return;
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.Instance.poolManager.ReturnManaDrop(this);
    }

    public void ReturnToPool()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        gameObject.SetActive(false);
    }
}
