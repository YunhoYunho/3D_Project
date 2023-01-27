using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private CrosshairUI crosshairUI;

    private float moveY = 0;
    private float originPosY;

    private float applyCrouchPosY;
    private float applySpeed;

    private bool isGround = true;
    private bool isJumping = false;
    private bool isCrouch = false;

    [Header("General")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float crouchPosY;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float gravity;

    [Header("InterAction")]
    [SerializeField]
    private bool interActionGizmos;
    [SerializeField]
    private float interActionRange;
    [SerializeField, Range(0f, 360f)]
    private float interActionAngle;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        crosshairUI = FindObjectOfType<CrosshairUI>();
    }

    private void Start()
    {
        applySpeed = moveSpeed;
        originPosY = Camera.main.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    private void Update()
    {
        IsGround();
        TryJump();
        TryCrouch();
        Move();
        InterAction();
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, controller.bounds.extents.y + 0.1f);
        crosshairUI.Jumping(!isGround);
    }

    private void Move()
    {
        controller.Move(transform.forward * InputManager.Instance.move * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * InputManager.Instance.rotate * moveSpeed * Time.deltaTime);
    }

    private void TryJump()
    {
        if (isCrouch)
            Crouch();

        Jump();
    }

    private void Jump()
    {
        if (InputManager.Instance.jump && isGround)
        {
            moveY = jumpPower;
            crosshairUI.Jumping(isJumping);
        }
        else if (controller.isGrounded)
            moveY = 0;
        else
            moveY += gravity * Time.deltaTime;

        controller.Move(Vector3.up * moveY * Time.deltaTime);
    }

    private void TryCrouch()
    {
        if (InputManager.Instance.crouch)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        crosshairUI.Crouching(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }

        else
        {
            applySpeed = moveSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    private IEnumerator CrouchCoroutine()
    {
        float posY = Camera.main.transform.localPosition.y;
        int count = 0;

        while (posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.2f);
            Camera.main.transform.localPosition = new Vector3(0, posY, 0);

            if (count > 30)
                break;
            yield return null;
        }

        Camera.main.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    private void InterAction()
    {
        if (!InputManager.Instance.interAction)
            return;

        Collider[] colliders = Physics.OverlapSphere(
            transform.position, interActionRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 dirToTarget = (
                colliders[i].transform.position - transform.position).normalized;

            if (Vector3.Dot(transform.forward, dirToTarget) < 
                Mathf.Cos(interActionAngle * 0.5f * Mathf.Deg2Rad))
                continue;

            IInteractable target = colliders[i].GetComponent<IInteractable>();
            target?.InterAction(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interActionGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interActionRange);

            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + interActionAngle * 0.5f);
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - interActionAngle * 0.5f);
            Debug.DrawRay(transform.position, rightDir * interActionRange, Color.blue);
            Debug.DrawRay(transform.position, leftDir * interActionRange, Color.blue);
        }
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
