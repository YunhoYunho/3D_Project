using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;

    public GameObject pausePanel;
    public GameObject gameoverText;
    public GameObject doorOpenText;
    public GameObject notEnoughKeyText;
    public GameObject winnerText;

    public TextMeshProUGUI curKeyText;

    public void AmmoTextUI(int magAmmo, int maxAmmo)
    {
        ammoText.text = magAmmo + "/" + maxAmmo;
    }

    public void HealthTextUI(float curHP, float maxHP)
    {
        healthText.text = curHP + " / " + maxHP;
    }

    public void KeyText(int curKey, int maxKey)
    {
        curKeyText.text = "현재 열쇠 개수 : " + curKey + " / " + maxKey;
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
