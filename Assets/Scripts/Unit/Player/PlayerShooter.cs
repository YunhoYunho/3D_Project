using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public enum Aim
    {
        Idle,
        Fire
    }

    [Header("Gun")]
    [SerializeField]
    private GunData gunData;
    [SerializeField]
    private Gun gun;

    public Aim aimState { get; private set; }
    private Animator animator;
    private Camera playerCamera;
    private float releasingAimTime = 2.0f;
    private float lastFireInputTime;
    private Vector3 aimPoint;

    // y축 각도 1도 이상 false, 1도 이하 true
    private bool linedUP => !(Mathf.Abs(playerCamera.transform.eulerAngles.y - 
        transform.eulerAngles.y) > 1f);
    private bool hasEnoughDistance => !Physics.Linecast(
        transform.position + Vector3.up * gun.firePoint.position.y, 
        gun.firePoint.position);

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        gun = GetComponentInChildren<Gun>();
        playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (InputManager.Instance.fire)
        {
            Shot();
        }

        else if (InputManager.Instance.reload)
        {
            Reload();
        }
    }

    private void Update()
    {
        UpdateAimToTarget();
        Angle();
        ChangeIdleAim();
        UpdateUI();
        UpdateCrosshair();
    }

    private void Angle()
    {
        var angle = playerCamera.transform.eulerAngles.x;

        // -90 ~ 90
        if (angle > 270.0f)
            angle -= 360.0f;

        // 0 ~ 1
        angle = angle / 180.0f * -1.0f + 0.5f;

        animator.SetFloat("Angle", angle);
    }

    private void ChangeIdleAim()
    {
        if (!InputManager.Instance.fire && Time.time >= gun.lastFireTime + releasingAimTime)
        {
            aimState = Aim.Idle;
        }
    }

    public void Shot()
    {
        if (aimState == Aim.Idle)
        {
            if (linedUP)
            {
                aimState = Aim.Fire;
            }
        }

        else if (aimState == Aim.Fire)
        {
            if (hasEnoughDistance)
            {
                if (gun.Fire(aimPoint))
                {
                    animator.SetTrigger("Shot");
                }
            }

            else
            {
                aimState = Aim.Idle; 
            }
        }
    }

    private void Reload()
    {
        if (gun.Reload())
        {
            animator.SetTrigger("Reload");
        }
    }

    private void UpdateAimToTarget()
    {
        RaycastHit hit;

        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));

        if (Physics.Raycast(ray, out hit, gunData.range))
        {
            aimPoint = hit.point;

            if (Physics.Linecast(gun.firePoint.position, hit.point, out hit))
            {
                aimPoint = hit.point;
            }
        }
        
        else
        {
            aimPoint = playerCamera.transform.position +
                playerCamera.transform.forward * gunData.range;
        }
    }

    private void UpdateUI()
    {
        if (gun == null || UIManager.Instance == null)
            return;

        UIManager.Instance.AmmoTextUI(gun.magAmmo, gun.ammoRemain);
    }

    private void UpdateCrosshair()
    {
        UIManager.Instance.SetActiveCrosshair(hasEnoughDistance);
        UIManager.Instance.UpdateCrosshair(aimPoint);
    }
}
