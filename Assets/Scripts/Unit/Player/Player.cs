using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : Unit
{
    private Animator animator;
    private PlayerController controller;
    private PlayerShooter shooter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        controller.enabled = true;
        shooter.enabled = true;
    }

    public override void RestoreHP(int restoreHP)
    {
        base.RestoreHP(restoreHP);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Dead()
    {
        base.Dead();

        animator.SetTrigger("Dead");
        controller.enabled = false;
        shooter.enabled = false;
    }

}
