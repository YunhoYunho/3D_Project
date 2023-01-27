using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int frameCount = 0;
    private float fps = 0;
    private float timeLeft = 0.5f;
    private float timePassed = 0f;
    private float updateInterval = 0.5f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (!text)
        {
            Debug.LogError("text 컴포넌트가 없습니다!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        frameCount += 1;
        timeLeft -= Time.deltaTime;
        timePassed += Time.timeScale / Time.deltaTime;

        if (timeLeft <= 0f)
        {
            fps = timePassed / frameCount;
            timeLeft = updateInterval;
            timePassed = 0f;
            frameCount = 0;
        }

        text.text = string.Format("{0} FPS", fps);
    }
}
