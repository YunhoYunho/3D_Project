using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State { Ready, Empty, Reloading }

    public State state { get; private set; }

    private Animator GunAnimator;
    public GunData gunData;
    public int magAmmo;
    private float lastFireTime;

    [SerializeField]
    private AudioSource gunAudioSource;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private ParticleSystem shellEject;

    private void Awake()
    {
        GunAnimator = GetComponent<Animator>();
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
            GunAnimator.SetTrigger("Fire");
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if (Physics.Raycast(Camera.main.transform.position,
            Camera.main.transform.forward, out hit, 200f))
        {
            IDamagable damagable = hit.transform.GetComponent<IDamagable>();
            damagable?.TakeDamage(gunData.damage);

            IBulletTakable bulletTakable = hit.transform.GetComponent<IBulletTakable>();
            bulletTakable?.TakeBullet(hit.point, hit.normal, gunData.bulletForce);
        }
        StartCoroutine(ShotEffect(hitPosition));

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
        gunAudioSource.PlayOneShot(gunData.shotClip);

        yield return new WaitForSeconds(0.03f);
    }

    public bool Reload()
    {
        if (state == State.Reloading || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        GunAnimator.SetTrigger("Reload");
        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;
        magAmmo += ammoToFill;
        state = State.Ready;
    }
}
