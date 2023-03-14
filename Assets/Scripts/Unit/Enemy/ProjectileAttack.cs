using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField]
    private Transform firePosition;
    [SerializeField]
    private GameObject arrow;

    public void CreateArrow()
    {
        Instantiate(arrow, firePosition.position, firePosition.rotation);
    }
}
