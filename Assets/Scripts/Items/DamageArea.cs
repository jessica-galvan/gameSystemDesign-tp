using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour, IDamage, IUpdate, IInstantiableAction
{
    public LayerMask targets;
    public AttackDataSO attackSO;

    private float timeLeft;

    public AttackDataSO AttackData => attackSO;

    public void Initialize(float timeAlive)
    {
        timeLeft = timeAlive;
        GameManager.Instance.updateManager.gameplayCustomUpdate.Add(this);
    }

    public void Refresh(float deltaTime)
    {
        timeLeft -= deltaTime;

        if (timeLeft <= 0)
            Die();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (MiscUtils.IsInLayerMask(collision.gameObject.layer, targets) && collision.TryGetComponent<IDamagable>(out var damagable))
            damagable.TakeDamage(attackSO.damage, ignoreCooldown: false);
    }

    private void Die()
    {
        GameManager.Instance.updateManager.gameplayCustomUpdate.Remove(this);
        Destroy(gameObject);
    }
}
