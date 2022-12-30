using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField]
    private Gun gun;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.fire)
        {
            gun.Fire();
        }

        else if (playerInput.reload)
        {
            gun.Reload();
        }
    }
}
