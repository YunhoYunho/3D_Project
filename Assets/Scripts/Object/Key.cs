using UnityEngine;

public class Key : MonoBehaviour, IItem
{
    public int score = 1;

    public void Use(GameObject item)
    {
        GameManager.Instance.Key(score);
        Destroy(gameObject);
    }
}
