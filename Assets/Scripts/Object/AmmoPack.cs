using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IItem
{
    [SerializeField]
    private AmmoPackData data; 

    public void Use(GameObject item)
    {
        var player = item.GetComponent<PlayerShooter>();
        var gun = item.GetComponent<Gun>();

        if (player != null)
        {
            gun.ammoRemain += data.ammoCount;
        }

        Destroy(gameObject);
    }
}
