using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private float moveY = 0;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float gravity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        controller.Move(transform.forward * playerInput.move * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * playerInput.rotate * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (playerInput.jump)
            moveY = jumpPower;
        else if (controller.isGrounded)
            moveY = 0;
        else
            moveY += gravity * Time.deltaTime;

        controller.Move(Vector3.up * moveY * Time.deltaTime);
    }
}
