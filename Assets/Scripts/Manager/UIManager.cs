using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public PlayerHealth player;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waveText; 
    public Slider healthSlider;
    public GameObject gameoverUI;

    public GameObject doorOpenText;
    public GameObject doorGoalText;

    public void AmmoTextUI(int magAmmo, int maxAmmo)
    {
        ammoText.text = magAmmo + "/" + maxAmmo;
    }

    public void HealthTextUI(float curHP, float maxHP)
    {
        healthText.text = curHP + "/" + maxHP;
    }

    public void WaveTextUI(int wave, int count)
    {
        waveText.text = "Wave : " + wave + "\nEnemy Left : " + count;
    }

    public void GameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    #region PopUP UI

    public void DoorOpen(bool active)
    {
        doorOpenText.SetActive(active);
    }

    public void DoorGoal(bool active)
    {
        doorGoalText.SetActive(active);
    }

    #endregion
}
