using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private void Update()
    {
        if (InputManager.Instance.pause)
        {
            if (!GameManager.isPause)
                OpenMenu();
            else
                CloseMenu();
        }
    }

    private void OpenMenu()
    {
        GameManager.isPause = true;
        UIManager.Instance.pausePanel.SetActive(true);
        GameManager.Instance.Pause();
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        UIManager.Instance.pausePanel.SetActive(false);
        GameManager.Instance.Resume();
    }
}
