using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour, IDamage
{
    public LayerMask targets;
    public AttackDataSO attackSO;
    public float timeAlive = 5f;

    private float timeLeft;

    public AttackDataSO AttackData => attackSO;

    //TODO implement custom update logic and pooling
    void Start()
    {
        timeLeft = timeAlive;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
            Die();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(MiscUtils.IsInLayerMask(collision.gameObject.layer, targets) && collision.TryGetComponent<IDamagable>(out var damagable))
            damagable.TakeDamage(attackSO.damage);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
