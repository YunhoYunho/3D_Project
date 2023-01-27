using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Unit
{
    private PlayerController controller;
    private PlayerShooter shooter;
    private AudioSource audioSource;

    [Header("Player")]
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private AudioClip hitClip;
    [SerializeField]
    private AudioClip deathClip;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShooter>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        UIManager.Instance.healthSlider.maxValue = playerData.hp;
        UIManager.Instance.healthSlider.value = hp;

        controller.enabled = true;
        shooter.enabled = true;
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);

        UIManager.Instance.healthSlider.value = hp;
        UIManager.Instance.HealthTextUI(hp, playerData.hp);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            audioSource.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        UIManager.Instance.healthSlider.value = hp;
        UIManager.Instance.HealthTextUI(hp, playerData.hp);
    }

    public override void Die()
    {
        base.Die();

        controller.enabled = false;
        shooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();

            if (item != null)
            {
                item.Use(gameObject);
            }
        }
    }
}
