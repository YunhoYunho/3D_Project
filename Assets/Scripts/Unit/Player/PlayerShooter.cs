using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator playerAnimator;

    [SerializeField]
    private Gun gun;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
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
