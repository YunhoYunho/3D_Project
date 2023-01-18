using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    public enum State { Ready, Empty, Reloading }

    public State state { get; private set; }

    public GunData gunData;
    private Animator gunAnimator;

    // HUD : ¿‹ø© ≈∫æ‡ / √÷¥Î ≈∫æ‡
    [Header("Spec")]
    [SerializeField]
    public int magAmmo;
    [SerializeField]
    private float fireRange;
    [SerializeField]
    private float lastFireTime;
    [SerializeField]
    private Transform firePosition;

    [Header("Effect")]
    [SerializeField]
    private Transform firingTransform;
    [SerializeField]
    private AudioSource gunAudioSource;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private ParticleSystem shellEject;

    private void Awake()
    {
        gunAnimator = GetComponent<Animator>();
        gunAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        magAmmo = gunData.magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();

            gunAnimator.SetTrigger("Fire");
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if (Physics.Raycast(firePosition.transform.position,
            firePosition.transform.forward, out hit, gunData.range))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * gunData.range;
        }

        StartCoroutine(ShotEffectCoroutine(hitPosition));

        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffectCoroutine(Vector3 hitPosition)
    {
        muzzleFlash.Play();
        shellEject.Play();
        gunAudioSource.PlayOneShot(gunData.shotClip);

        yield return new WaitForSeconds(0.03f);
    }

    public bool Reload()
    {
        if (state == State.Reloading || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        gunAnimator.SetTrigger("Reload");

        StartCoroutine(ReloadCoroutine());
        return true;
    }

    private IEnumerator ReloadCoroutine()
    {
        state = State.Reloading;
        gunAudioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;
        magAmmo += ammoToFill;
        state = State.Ready;
    }
}
