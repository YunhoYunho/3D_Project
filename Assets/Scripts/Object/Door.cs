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
    }
}
