using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public float timeBetFire = 0.12f;
    public float reloadTime = 1.8f;

    [SerializeField]
    public int damage = 25;

    [SerializeField]
    public int magCapacity = 30;

    [SerializeField]
    public float bulletForce = 1;
}
