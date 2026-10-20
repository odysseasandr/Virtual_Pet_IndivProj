using UnityEngine;
using System.Collections;

public class BathManager : MonoBehaviour
{
    [Header("References")]
    public CoinManager coinManager;
    public PetState petState;

    [Header("UI Menu")]
    public GameObject bathmenuPanel;

    [Header("Pet Animator")]
    public Animator petAnimator;

    [Header ("Bath Visual")]
    public GameObject hoseProp;
    public GameObject spongeProp;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip hoseSound;
    public AudioClip spongeSound;

    public void ToggleBathMenu()
    {
        if (bathmenuPanel != null) bathmenuPanel.SetActive(!bathmenuPanel.activeSelf);
    }

    public void BathwHose(){CleanPet(0);}
    public void BathwSponge(){CleanPet(1);}
    
    private void CleanPet(int bathType)
    {
        if (petState != null && (petState.isSleeping || petState.isBusy)) 
        {   
            Debug.Log("Cannot bathe while pet is asleep"); 
            return;
        }

        if (GameObject.FindGameObjectWithTag("Food") != null) 
        {
            Debug.Log("Pet should eat its food before the bath"); 
            return;
        }


        int cost = (bathType == 0) ? 0 : 5;
        string bathMethod = (bathType == 0) ? "Hose" : "Sponge";

        if (coinManager != null && coinManager.CanSpendCoins(cost))
        {
            Debug.Log("Cleaned pet with " + bathMethod);
            if (bathmenuPanel != null) bathmenuPanel.SetActive(false);

            float currentAudioLength = 4f;

            if (petState != null)
            {
                petState.clean = 100f;

                if (bathType == 0)
                {
                    petState.fear = Mathf.Clamp(petState.fear + 100f, 0f, 1000f);
                    petState.happy = Mathf.Clamp(petState.happy - 5f, 0f, 100f);
                }
            }
        
            if (petAnimator != null)
            {
                if (bathType == 0)
                {
                    petAnimator.SetTrigger("HBath");
                    if (sfxSource != null && hoseSound != null)
                    { 
                        sfxSource.PlayOneShot(hoseSound);
                    }
                    StartCoroutine(HandleBathRoutine(hoseProp, currentAudioLength));
                }
                else
                {
                    
                    petAnimator.SetTrigger("SBath");
                    if (sfxSource != null && spongeSound != null)
                    {
                        sfxSource.PlayOneShot(spongeSound);
                        currentAudioLength = spongeSound.length;
                    }
                    StartCoroutine(HandleBathRoutine(spongeProp, currentAudioLength));
                }
            }
        }
    }

    private IEnumerator HandleBathRoutine(GameObject prop, float duration)
    {
        if (petState != null) petState.isBusy = true;

        if (prop != null) prop.SetActive(true);

        yield return new WaitForSeconds(duration);
        
        if (prop != null) prop.SetActive(false);

        if(petState != null) petState.isBusy = false;

        if (petAnimator != null) petAnimator.SetTrigger("Idle");
    }
    
}
