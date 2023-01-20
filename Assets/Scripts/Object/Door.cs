using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private readonly int hashTouch = Animator.StringToHash("Touch");

    public void Touch()
    {
        animator.SetTrigger(hashTouch);
        Debug.Log(string.Format("{0}이 열림.", gameObject.name));
    }

    public void Goal()
    {
        Debug.Log(string.Format("{0}에 도착함.", gameObject.name));
        GameManager.Instance.SecondScene();
    }
}
