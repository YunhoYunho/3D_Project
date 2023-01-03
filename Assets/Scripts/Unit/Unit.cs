using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    public float defaultHP = 100f;

    public float hp { get; protected set; }

    public bool dead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        hp = defaultHP;
    }

    public virtual void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <=0 && !dead)
        {
            Dead();
        }
    }

    public virtual void RestoreHP(int restoreHP)
    {
        if (dead)
        {
            return;
        }

        hp += restoreHP;
    }

    public virtual void Dead()
    {
        if (onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
}
