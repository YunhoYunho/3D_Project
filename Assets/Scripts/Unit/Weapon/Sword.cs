using UnityEngine;

public class Sword : MonoBehaviour
{
    private new Collider collider;

    [SerializeField]
    private EnemyData enemyData;

    private void Awake()
    {
        collider = GetComponentInChildren<Collider>();
    }

    public void EnableCollider()
    {
        collider.enabled = true;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }


}
