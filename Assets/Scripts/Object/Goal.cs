using UnityEngine;

public class Goal : MonoBehaviour
{
    public void ArriveGoal()
    {
        if (GameManager.Instance.isKeyEnough)
        {
            UIManager.Instance.Goal(true);
        }

        else if (false == GameManager.Instance.isKeyEnough)
        {
            UIManager.Instance.Goal(false);
        }
    }
}
