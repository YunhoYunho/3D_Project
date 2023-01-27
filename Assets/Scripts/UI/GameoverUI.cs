using UnityEngine;

public class GameoverUI : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private readonly int hashactive = Animator.StringToHash("Gameover");

    private void Update()
    {
        if (GameManager.Instance.IsGameover)
        {
            GameoverAnimator();
        }
    }

    private void GameoverAnimator()
    {
        animator.SetTrigger(hashactive);
    }
}
