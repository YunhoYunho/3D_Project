using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool IsGameover { get; private set; } // 게임 오버 상태
    public static bool isPause = false;
    public bool canPlayerMove = true;

    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDeath += Gameover;
    }

    private void Update()
    {
        if (isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            canPlayerMove = false;
        }

        else
        {

        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Gameover()
    {
        Pause();
        IsGameover = true;
        UIManager.Instance.GameoverUI(true);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SecondScene()
    {
        SceneManager.LoadScene("Sector2");
    }
}
