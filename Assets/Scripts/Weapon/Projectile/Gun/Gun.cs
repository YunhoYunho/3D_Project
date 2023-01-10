using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State { Ready, Empty, Reloading }

    public State state { get; private set; }

    public ProjectileData weaponData;
    private Animator gunAnim;
    private RaycastHit hit;

    // HUD : ¿‹ø© ≈∫æ‡ / √÷¥Î ≈∫æ‡
    [Header("Spec")]
    [SerializeField]
    public int magAmmo;
    [SerializeField]
    private float fireRange;
    [SerializeField]
    private float lastFireTime;

    [Header("Effect")]
    [SerializeField]
    private Transform firingTransform;
    [SerializeField]
    private AudioSource gunAudioSource;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private ParticleSystem shellEject;
    [SerializeField]
    private GameObject hitEffectPrefab;

    private void Awake()
    {
        gunAnim = GetComponent<Animator>();
        gunAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        magAmmo = weaponData.magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + weaponData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();

            gunAnim.SetTrigger("Fire");
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if (Physics.Raycast(Camera.main.transform.position,
            Camera.main.transform.forward, out hit, fireRange))
        {
            IDamagable damagable = hit.transform.GetComponent<IDamagable>();
            damagable?.TakeDamage(weaponData.damage);

            IBulletTakable bulletTakable = hit.transform.GetComponent<IBulletTakable>();
            bulletTakable?.TakeBullet(hit.point, hit.normal, weaponData.bulletForce);
        }

        StartCoroutine(ShotEffect(hitPosition));

        StopAllCoroutines();
        
        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlash.Play();
        shellEject.Play();
        gunAudioSource.PlayOneShot(weaponData.shotClip);

        yield return new WaitForSeconds(0.03f);
    }

    public bool Reload()
    {
        if (state == State.Reloading || magAmmo >= weaponData.magCapacity)
        {
            return false;
        }

        gunAnim.SetTrigger("Reload");

        StartCoroutine(ReloadCoroutine());
        return true;
    }

    private IEnumerator ReloadCoroutine()
    {
        state = State.Reloading;
        gunAudioSource.PlayOneShot(weaponData.reloadClip);

        yield return new WaitForSeconds(weaponData.reloadTime);

        int ammoToFill = weaponData.magCapacity - magAmmo;
        magAmmo += ammoToFill;
        state = State.Ready;
    }
}
