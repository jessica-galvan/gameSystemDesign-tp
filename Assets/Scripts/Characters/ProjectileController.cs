using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour, IUpdate
{
    public ProjectileDataSO data;

    private Rigidbody2D body;
    private Vector2 direction;
    private float timer;
    private bool active;

    public void Initialize()
    {
        body = GetComponent<Rigidbody2D>();
        transform.position = Vector2.zero;
    }

    public void SetDirection(Vector2 startingPosition, Vector2 direction, float rotation = 0f)
    {
        transform.position = startingPosition;
        
        this.direction = direction;
        transform.eulerAngles = Vector3.forward * rotation;
        print(transform.eulerAngles);
        timer = 0f;
        SetVisibility(true);
        active = true;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;

        if (collision.gameObject.TryGetComponent<IDamagable>(out var damagable))
            damagable.TakeDamage(data.baseDamage, direction); //TODO: add multipliers

        Die();
    }

    public void Refresh()
    {
        if (!active) return;
        if (GameManager.Instance.Pause || GameManager.Instance.Won) return;
        body.velocity += direction * data.speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer > data.totalTimeAlive)
            Die();
    }

    //public void FixedRefresh()
    //{
    //    if (!active) return;
    //    if (GameManager.Instance.Pause || GameManager.Instance.Won) return;
    //    body.velocity += direction * data.speed * Time.deltaTime;
    //}

    public void ReturnToPool()
    {
        SetVisibility(false);
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        transform.position = Vector2.zero;
    }

    private void SetVisibility(bool value)
    {
        active = value;
        gameObject.SetActive(value);
    }

    private void Die()
    {
        ReturnToPool();
        Destroy(gameObject);

        //var impactVFX = GameManager.Instance.poolManager.GethParticle(ParticleController.ParticleType.BulletImpact);
        //impactVFX.Spawn(transform);
        //GameManager.Instance.poolManager.ReturnBullet(this);
    }
}
