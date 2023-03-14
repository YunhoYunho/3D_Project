using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    private float moveY = 0;

    private bool isGround = true;

    [Header("General")]
    [SerializeField]
    private float moveSpeed;
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
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        IsGround();
        Move();
        Rotate();
        Jump();
        InterAction();
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, controller.bounds.extents.y + 0.1f);
    }

    private void Move()
    {
        Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        animator.SetFloat("XInput", Input.GetAxis("Horizontal"));
        animator.SetFloat("YInput", Input.GetAxis("Vertical"));

        Vector3 moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;

        controller.Move(moveVec * moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        transform.forward = new Vector3(
            Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
    }

    private void Jump()
    {
        if (InputManager.Instance.jump && isGround)
        {
            moveY = jumpPower;
        }
        else if (controller.isGrounded)
            moveY = 0;
        else
            moveY += gravity * Time.deltaTime;

        controller.Move(Vector3.up * moveY * Time.deltaTime);
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
