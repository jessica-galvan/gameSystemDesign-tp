using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour, IPoolable, IUpdate
{
    public enum ParticleType
    {
        Death,
        OnHit
    }

    [SerializeField] private ParticleSystem[] allParticles;
    [SerializeField] private float timeAlive = 2f;

    [field:SerializeField ] public ParticleType Type;

    private float currentTimer = 0f;

    public void Initialize() { }

    public void ReturnToPool() 
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
    }

    public void Set(Transform transform)
    {
        this.transform.position = transform.position;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
        currentTimer = timeAlive;
        PlayVFX();
    }

    public void PlayVFX()
    {
        for (int i = 0; i < allParticles.Length; i++)
            allParticles[i].Play();
    }

    public void Refresh()
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.Instance.poolManager.ReturnParticle(this);
    }
}
