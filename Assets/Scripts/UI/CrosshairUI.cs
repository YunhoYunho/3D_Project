using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private readonly int hashJump = Animator.StringToHash("Jumping");
    private readonly int hashCrouch = Animator.StringToHash("Crouching");

    public void Jumping(bool flag)
    {
        animator.SetBool(hashJump, flag);
    }

    public void Crouching(bool flag)
    {
        animator.SetBool(hashCrouch, flag);
    }
}
