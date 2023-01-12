using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public HealthPackData data;

    public Player player;
    public void Use(GameObject item)
    {
        Player hp = item.GetComponent<Player>();

        if (hp != null)
        {
            hp.RestoreHP(data.healthPoint);
        }

        Destroy(gameObject);
    }
}
