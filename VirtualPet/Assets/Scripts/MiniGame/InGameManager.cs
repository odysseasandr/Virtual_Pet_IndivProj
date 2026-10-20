using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI coinT;

    [Header("Audio Settings")]
    public AudioSource bgmSource;
    public AudioClip normalMusic;
    public AudioClip creepyMusic;
    public float fearThreshold = 700f;

    //coin counter
    private int collectCoins= 0;

    void Start()
    {
        float currentFear = PlayerPrefs.GetFloat("fear", 0f);

        if (bgmSource != null)
        {
            if (currentFear >= fearThreshold && creepyMusic != null)
            {
                bgmSource.clip = creepyMusic;
            }
            else if (normalMusic != null)
            {
                bgmSource.clip = normalMusic;
            }
            bgmSource.Play();
        }
    }

    public void AddCoins()
    {
        collectCoins ++;
        coinT.text = "Coins: " + collectCoins;
    }


    //save coins to wallet
    private void SaveCoins()
    {   //check coins from previous runs
        int prevCoins = PlayerPrefs.GetInt("PendingCoins", 0);
        //add new coins to previous 
        PlayerPrefs.SetInt("PendingCoins", prevCoins + collectCoins);

        float earnedHappiness = 5 + (collectCoins * 2);
        float prevHappiness = PlayerPrefs.GetFloat("PendingHappy", 0f);
        PlayerPrefs.SetFloat("PendingHappy", prevHappiness + earnedHappiness);

        //save
        PlayerPrefs.Save();

        Debug.Log("Saved" + collectCoins + " coins to wallet and " + earnedHappiness + " happiness");
    }

    public void RestartGame()
    {
        SaveCoins(); //save

        float prevEnergySpent = PlayerPrefs.GetFloat("PendingEnergySpent", 0f);
        PlayerPrefs.SetFloat("PendingEnergySpent", prevEnergySpent + 5f);
        PlayerPrefs.Save();
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMain()
    {
        SaveCoins();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}
