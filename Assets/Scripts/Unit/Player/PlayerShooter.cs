using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField]
    private GunData gunData;
    [SerializeField]
    private Gun gun;
    [SerializeField]
    private GameObject hitEffect;

    private void Update()
    {
        Debug.DrawRay(Camera.main.transform.position,
            Camera.main.transform.forward * gunData.range,
            Color.green);

        if (InputManager.Instance.fire)
        {
            gun.Fire();
        }

        else if (InputManager.Instance.reload)
        {
            gun.Reload();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gun != null && UIManager.Instance != null)
        {
            UIManager.Instance.AmmoTextUI(gun.magAmmo, gunData.magCapacity);
        }
    }
}
