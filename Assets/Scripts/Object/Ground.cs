using UnityEngine;

public class Ground : MonoBehaviour, IBulletTakable
{
    [SerializeField]
    private ParticleSystem bulletEffect;

    public void TakeBullet(Vector3 hitPoint, Vector3 hitNormal, float hitForce)
    {
        ParticleSystem particle = Instantiate(bulletEffect, hitPoint, Quaternion.LookRotation(hitNormal));
        particle.transform.parent = transform;
    }
}
