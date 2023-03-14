using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IDamageable
{
    protected float initHP = 200.0f;
    public float hp { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        hp = initHP;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp -= damage;

        if (hp <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void RestoreHP(float restoreHP)
    {
        if (dead)
        {
            return;
        }

        hp += restoreHP;
    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
}
