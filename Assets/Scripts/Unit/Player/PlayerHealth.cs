using UnityEngine;

public class PlayerHealth : Unit
{
    private PlayerController controller;
    private PlayerShooter shooter;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Player")]
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private AudioClip hitClip;
    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    private AudioClip healingClip;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShooter>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        controller.enabled = true;
        shooter.enabled = true;

        UIManager.Instance.healthSlider.maxValue = playerData.hp;
        UpateUI();
    }

    public override void RestoreHP(float restoreHP)
    {
        base.RestoreHP(restoreHP);

        UpateUI();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            audioSource.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);

        UpateUI();
    }

    public override void Die()
    {
        base.Die();

        animator.SetTrigger("Die");
        audioSource.PlayOneShot(deathClip);

        controller.enabled = false;
        shooter.enabled = false;
    }

    private void UpateUI()
    {
        UIManager.Instance.healthSlider.value = hp;
        UIManager.Instance.HealthTextUI(hp, playerData.hp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            IItem item = other.GetComponent<IItem>();

            if (item != null)
            {
                item.Use(gameObject);
                audioSource.PlayOneShot(healingClip);
            }
        }
    }
}
