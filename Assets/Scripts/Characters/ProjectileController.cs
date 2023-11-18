using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Basic
}

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour, IUpdate, IPoolable, IDamage, IDirection
{
    public ProjectileDataSO data;
    public AttackDataSO attackData;

    private Rigidbody2D body;
    private Vector2 direction;
    private float timer;
    private bool active;

    public AttackDataSO AttackData => attackData;

    public void Initialize()
    {
        attackData.Initialize();
        body = GetComponent<Rigidbody2D>();
        transform.position = Vector2.zero;
    }

    public void SetDirection(Vector2 startingPosition, Vector2 direction, float rotation = 0f)
    {
        transform.position = startingPosition;
        
        this.direction = direction.normalized;
        transform.eulerAngles = Vector3.forward * rotation;
        timer = 0f;
        SetVisibility(true);
        active = true;
        body.velocity = this.direction * data.StartingSpeed;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;

        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
        {
            if(attackData.force > 0)
            {
                var knockback = direction.normalized * attackData.force;
                damagable.TakeDamage(attackData.Damage, knockback); //TODO: add multipliers
            }
            else
                damagable.TakeDamage(attackData.Damage); //TODO: add multipliers

        }

        Die();
    }

    public void Refresh(float deltaTime)
    {
        if (!active) return;
        if (!GameManager.Instance.CanUpdate) return;
        body.velocity += direction * data.Speed * deltaTime;

        if (!GameManager.Instance.cameraController.IsInsideFrustrum(transform.position))
            Die();

        timer += deltaTime;
        if (timer > data.TimeAlive)
            Die();
    }

    public void ReturnToPool()
    {
        //var impactVFX = GameManager.Instance.poolManager.GethParticle(ParticleController.ParticleType.BulletImpact);
        //impactVFX.Spawn(transform);

        SetVisibility(false);
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        transform.position = Vector2.zero;
    }

    private void SetVisibility(bool value)
    {
        active = value;
        gameObject.SetActive(value);
    }

    private void Die(bool timeOver = false)
    {
        if(timeOver && data.bulletDestructionVFX != null)
            Instantiate(data.bulletDestructionVFX);

        GameManager.Instance.poolManager.ReturnBullet(this);
    }
}
