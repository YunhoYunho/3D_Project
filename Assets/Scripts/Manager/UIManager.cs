using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI curKeyText;
    public TextMeshProUGUI waveText;
    public Slider healthSlider;

    public GameObject pausePanel;
    public GameObject gameoverText;
    public GameObject doorOpenText;
    public GameObject notEnoughKeyText;
    public GameObject winnerText;

    public CrosshairUI crosshair;

    public void AmmoTextUI(int magAmmo, int maxAmmo)
    {
        ammoText.text = string.Format(" {0} / {1} ", magAmmo, maxAmmo);
    }

    public void HealthTextUI(float curHP, float maxHP)
    {
        healthText.text = string.Format(" {0} / {1} ", curHP, maxHP);
    }

    public void WaveText(int wave, int count)
    {
        waveText.text = string.Format("현재 Wave : {0} / 남은 적군 수 : {1}", wave, count);
    }

    public void KeyText(int curKey, int maxKey)
    {
        curKeyText.text = string.Format("현재 열쇠 개수 : {0} / {1}", curKey, maxKey);
    }

    public void UpdateCrosshair(Vector3 worldPosition)
    {
        crosshair.UpdatePosition(worldPosition);
    }

    public void SetActiveCrosshair(bool active)
    {
        crosshair.SetActiveCrosshair(active);
    }

    #region PopUP UI

    public void GameoverUI(bool active)
    {
        gameoverText.SetActive(active);
    }

    public void Goal(bool active)
    {
        if (GameManager.Instance.isKeyEnough == true)
        {
            winnerText.SetActive(active);
        }

        else if (GameManager.Instance.isKeyEnough == false)
        {
            notEnoughKeyText.SetActive(active);
        }
    }

    #endregion
}
