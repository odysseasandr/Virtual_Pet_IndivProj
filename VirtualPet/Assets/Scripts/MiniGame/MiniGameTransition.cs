using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameTransition : MonoBehaviour
{
    [Header("References")]
    public PetState petState;
    public CoinManager coinManager;

    [Header("UI Elements")]
    public GameObject confirmPopup;

    void Start()
    {
        CheckForRewards();
    }

    public void OpenConfirm()
    {
        confirmPopup.SetActive(true);
    }

    public void CloseConfirm()
    {
        confirmPopup.SetActive(false);
    }

    public void StartMinigame()
    {
        petState.SpendEnergy(20f);

        //load minigame
        SceneManager.LoadScene("MiniGame");
    }

    //return from minigame

    private void CheckForRewards()
    {
        if (PlayerPrefs.HasKey("PendingCoins"))
        {
            int earnedCoins = PlayerPrefs.GetInt("PendingCoins");
            float earnedHappiness = PlayerPrefs.GetFloat("PendingHappy");

            //apply using CoinManager and PetState
            coinManager.AddCoins(earnedCoins);
            petState.happy += earnedHappiness; 

            PlayerPrefs.DeleteKey("PendingCoins");
            PlayerPrefs.DeleteKey("PendingHappy");
            PlayerPrefs.Save();

            //save new updated state
            petState.SaveState();

            Debug.Log($"Minigame rewards: {earnedCoins} coins and {earnedHappiness} in happiness");

        }

        if (PlayerPrefs.HasKey("PendingEnergySpent"))
        {
            float energyToSpend = PlayerPrefs.GetFloat("PendingEnergySpent");

            petState.SpendEnergy(energyToSpend);
            PlayerPrefs.DeleteKey("PendingEnergySpent");
            PlayerPrefs.Save();
        }
        petState.SaveState();
    }
}
