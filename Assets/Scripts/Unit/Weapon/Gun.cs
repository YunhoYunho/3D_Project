using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    public enum State { Ready, Empty, Reloading }
    public State state { get; private set; }
    private Animator gunAnimator;

    [Space]

    [Header("Data")]
    [SerializeField]
    private GunData gunData;
    [SerializeField]
    private Transform firePosition;

    [Space]
    
    [Header("Spec")]
    public int magAmmo;
    public int ammoRemain;
    [SerializeField]
    private float fireRange;
    [SerializeField]
    private float lastFireTime;

    [Space]

    [Header("Effect")]
    [SerializeField]
    private AudioSource gunAudioSource;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private ParticleSystem shellEject;
    [SerializeField]
    private LineRenderer lineRenderer; 
    
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private void Awake()
    {
        gunAnimator = GetComponent<Animator>();
        gunAudioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.initAmmo;
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

            gunAnimator.SetTrigger(hashFire);
        }
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(firePosition.position,
            firePosition.forward, out hit, gunData.range))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            IBulletTakable bulletTakable = hit.transform.GetComponent<IBulletTakable>();
            bulletTakable?.TakeBullet(hit.point, hit.normal, gunData.bulletForce);

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

        lineRenderer.SetPosition(0, firePosition.position);
        lineRenderer.SetPosition(1, hitPosition);
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        gunAnimator.SetTrigger(hashReload);

        StartCoroutine(ReloadCoroutine());

        return true;
    }

    private IEnumerator ReloadCoroutine()
    {
        state = State.Reloading;
        gunAudioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        int ammoToFill = gunData.magCapacity - magAmmo;

        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        state = State.Ready;
    }
}
