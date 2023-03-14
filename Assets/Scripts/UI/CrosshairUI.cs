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

    // 월드 좌표계 -> 화면상 좌표계 변환
    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint);
    }

    private void Update()
    {
        if (!hitPoint.enabled)
            return;

        // 기존 위치 -> 화면상 위치로 갱신, 참조접근으로 실시간 적용
        hitPointRectTransform.position = Vector2.SmoothDamp(
            hitPointRectTransform.position, targetPoint, ref hitPointVelocity,
            smoothTime * Time.deltaTime);
    }
}
