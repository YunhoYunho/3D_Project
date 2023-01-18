using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject crosshairHUD;

    [SerializeField]
    private Gun gun;

    public void Jumping(bool flag)
    {
        animator.SetBool("Jumping", flag);
    }

    public void Crouching(bool flag)
    {
        animator.SetBool("Crouching", flag);
    }
}
