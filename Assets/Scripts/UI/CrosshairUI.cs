using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject CrosshairHUD;

    [SerializeField]
    private Gun gun;

    [SerializeField]
    private float gunRecoil;

    public void Moving(bool flag)
    {
        animator.SetBool("Moving", flag);
    }

    public void Crouching(bool flag)
    {
        animator.SetBool("Crouching", flag);
    }

    public void Fire()
    {
        if (animator.GetBool("Moving"))
        {
            animator.SetTrigger("Move_Fire");
        }

        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("Crouch_Fire");
        }

        else
        {
            animator.SetTrigger("Idle_Fire");
        }
    }

    public float GetRecoil()
    {
        if (animator.GetBool("Moving"))
        {
            gunRecoil = 0.05f;
        }

        else if (animator.GetBool("Crouching"))
        {
            gunRecoil = 0.02f;
        }

        else
        {
            gunRecoil = 0.03f;

        }

        return gunRecoil;
    }
}
