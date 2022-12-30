using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAt : MonoBehaviour
{
    private PlayerInput playerInput;
    private float xRotation = 0f;

    [SerializeField]
    private float mouseSensitivity;

    [SerializeField]
    private Transform viewPoint;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Rotate();
        LookAt();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * playerInput.mouseX * mouseSensitivity * Time.deltaTime, Space.World);
    }

    private void LookAt()
    {
        xRotation -= playerInput.mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        viewPoint.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
