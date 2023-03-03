using UnityEngine;

// 초기 설정 값
[CreateAssetMenu(menuName = "Scriptable/Weapon/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;

    [Header("Spec")]
    [Range(0, 25)]
    public int damage = 25;
    [Range(0f, 200f)]
    public float range = 200f;
    [Range(0f, 1f)]
    public float timeBetFire = 0.12f;
    [Range(0f, 5f)]
    public float reloadTime = 1.8f;
    [Range(0f, 1f)]
    public float bulletForce = 1.0f;
    [Space]
    [Range(0f, 100f)]
    public int initAmmo = 100;
    [Range(0, 30)]
    public int magCapacity = 30;
}
