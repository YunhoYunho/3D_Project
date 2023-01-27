using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool IsGameover { get; private set; }
    public static bool isPause = false;
    public bool canPlayerMove = true;
    public bool isKeyEnough = false;

    public int curKey = 0;
    public int maxKey = 3;

    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDeath += Gameover;
    }

    private void Update()
    {
        if (isPause)
        {
            Pause();
        }

        if (IsGameover)
        {
            Gameover();
        }
    }

    public void Gameover()
    {
        IsGameover = true;
        UIManager.Instance.GameoverUI(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canPlayerMove = false;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Key(int getKey)
    {
        if (!IsGameover)
        {
            curKey += getKey;
            UIManager.Instance.KeyText(curKey, maxKey);

            if (curKey == maxKey)
            {
                isKeyEnough = true;
            }
        }
    }
}
