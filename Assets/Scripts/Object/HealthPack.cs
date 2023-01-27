using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    [SerializeField]
    private HealthPackData data;

    public void Use(GameObject item)
    {
        PlayerHealth hp = item.GetComponent<PlayerHealth>();

        if (hp != null)
        {
            hp.RestoreHP(data.healthPoint);
        }

        Destroy(gameObject);
    }
}
