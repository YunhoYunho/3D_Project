using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 초기 설정 값
[CreateAssetMenu(menuName = "Scriptable/Weapon/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;

    [Header("Spec")]
    [SerializeField]
    [Range(0, 25)]
    public int damage = 25;

    [SerializeField]
    [Range(0f, 200f)]
    public float range = 200f; 

    [SerializeField]
    public float bulletForce = 1;

    [SerializeField]
    [Range(0f, 1f)]
    public float timeBetFire = 0.12f;

    [SerializeField]
    [Range(0f, 5f)]
    public float reloadTime = 1.8f;

    [SerializeField]
    public float recoilForce;

    [SerializeField]
    [Range(0, 30)]
    public int magCapacity = 30;
}
