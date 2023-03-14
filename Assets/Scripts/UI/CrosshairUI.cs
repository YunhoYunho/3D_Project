using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField]
    private Image centerPoint;
    [SerializeField]
    private Image hitPoint;
    [SerializeField]
    private float smoothTime = 0.2f;

    private Vector2 hitPointVelocity;
    private Camera screenCamera;
    private RectTransform hitPointRectTransform;

    private Vector2 targetPoint;

    private void Awake()
    {
        screenCamera = Camera.main;
        hitPointRectTransform = hitPoint.GetComponent<RectTransform>();
    }

    public void SetActiveCrosshair(bool active)
    {
        hitPoint.enabled = active;
        centerPoint.enabled = active;
    }

    // ���� ��ǥ�� -> ȭ��� ��ǥ�� ��ȯ
    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint);
    }

    private void Update()
    {
        if (!hitPoint.enabled)
            return;

        // ���� ��ġ -> ȭ��� ��ġ�� ����, ������������ �ǽð� ����
        hitPointRectTransform.position = Vector2.SmoothDamp(
            hitPointRectTransform.position, targetPoint, ref hitPointVelocity,
            smoothTime * Time.deltaTime);
    }
}
