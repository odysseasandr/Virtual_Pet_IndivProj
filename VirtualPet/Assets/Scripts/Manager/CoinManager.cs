using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI coinText;

    public Color32 coinColor = new Color32(58, 82, 181, 255);

    [Header("Currency")]
    public int coins = 20;

    private void Start()
    {
        LoadCoins();
        UpdateCoinUI();

        if (coinText != null)
        {
            coinText.color = coinColor; 
        }
    }

    public bool CanSpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveCoins();
            UpdateCoinUI();
            return true;
        }

        Debug.Log("not enough coins");
        return false;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCoins();
        Debug.Log("Earned " + amount + " coins. The total is: " + "coins");
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coins.ToString();
        }
    }

    private void LoadCoins()
    {
        coins = PlayerPrefs.GetInt("SavedCoins", 20);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("SavedCoins", coins);
        PlayerPrefs.Save();
    }
}