using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public string startSceneName = "GameScene";

    public void ClickStart()
    {
        Debug.Log("Start");
        SceneManager.LoadScene(startSceneName);
    }

    public void ClickExit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
