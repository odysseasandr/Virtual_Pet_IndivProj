using UnityEngine;
using System;

public class PetState : MonoBehaviour
{
    //physical needs
    [Header("Physical Needs")]
    public float happy;
    public float energy;
    public float hunger;
    public float clean;

    //long-term hidden
    [Header("Hidden Behavior")]
    public float trust;
    public float fear;
    public float dependence;
    public float sanity;

    [Header("Sleep State")]
    public bool isSleeping = false;
    public bool manualSleep = false;

    [Header ("Action State")]
    public bool isBusy = false;

    private float drainRate = 0.0555f;
    //sleep approx 1hour
    private float restoreRate = 0.400f; 

    private void Awake()
    {
        LoadState();
        CalculateTimeOffline();
    }

    private float autoSaveT = 60f;
    private void Update()
    {
        ApplyTimePassage(Time.deltaTime);

        autoSaveT -= Time.deltaTime;
        if (autoSaveT <= 0)
        {
            SaveState();
            autoSaveT = 60f;
        }
    }

    private void CalculateTimeOffline()
    {
        if (PlayerPrefs.HasKey("LastSaveTime"))
        {
            string savedTime = PlayerPrefs.GetString("LastSaveTime");
            DateTime lastSaveTime;

            if (DateTime.TryParse(savedTime, out lastSaveTime))
            {
                TimeSpan timePassed = DateTime.Now - lastSaveTime;
                float secondsOffline = (float)timePassed.TotalSeconds;

                Debug.Log($"You were offline for {secondsOffline} seconds");

                ApplyTimePassage(secondsOffline);

                if (isSleeping && energy >= 100f)
                {
                    isSleeping = false;
                    Debug.Log("Pet fully rested online and woke up");
                    SaveState();
                }
            }
        }
    }

    private void ApplyTimePassage(float timeAmount)
    {
        if (isSleeping)
        {
            energy += restoreRate*timeAmount;
            //drainRate slower when asleep
            hunger += drainRate*0.5f*timeAmount;
            happy -= drainRate*0.5f*timeAmount;
            clean -= drainRate*0.5f*timeAmount;
        }
        else
        {
            hunger += drainRate*timeAmount;
            energy -= drainRate*timeAmount;
            happy -= drainRate*timeAmount;
            clean -= drainRate*timeAmount;
            
            float trustPH = 0f;
            float sanityPH = 0f;
            float fearPH = 0f;
            float dependencePH = 0f;
            if (energy <= 0f)
            {
                fearPH += 40f;
                dependencePH += 30f;
            }
            if (hunger <= 0f)
            {
                fearPH += 20f;
                dependencePH +=10f;
            }
            if (clean <= 0f)
            {
                fearPH += 15f;
                sanityPH += 10f;
            }
            if (happy <= 0f)
            {
                fearPH += 10f;
                dependencePH +=10;
            }
            if (trustPH > 0 || sanityPH > 0 || fearPH > 0 || dependencePH > 0)
            {
                trust -= trustPH/3600f*timeAmount;
                sanity -= sanityPH/3600f*timeAmount;
                fear += fearPH/3600f*timeAmount;
                dependence += dependencePH/3600f*timeAmount;
            }
        }

        ClampStats();
    }

    public void ClampStats()
    {
        happy = Mathf.Clamp(happy, 0f, 100f);
        energy = Mathf.Clamp(energy, 0f, 100f);
        hunger = Mathf.Clamp(hunger, 0f, 100f);
        clean = Mathf.Clamp(clean, 0f, 100f);

        trust = Mathf.Clamp(trust, 0f, 1000f);
        fear = Mathf.Clamp(fear, 0f, 1000f);
        dependence = Mathf.Clamp(dependence, 0f, 1000f);
        sanity = Mathf.Clamp(sanity, 0f, 1000f);

    }

    //save and load 
    public void LoadState()
    {
        happy = PlayerPrefs.GetFloat("SavedHappy", 70f);
        energy = PlayerPrefs.GetFloat("SavedEnergy", 100f);
        hunger = PlayerPrefs.GetFloat("SavedHunger", 90f);
        clean = PlayerPrefs.GetFloat("SavedClean", 10f);
        isSleeping = PlayerPrefs.GetInt("SavedSleep", 0) == 1;

        trust = PlayerPrefs.GetFloat("SavedTrust", 400f);
        fear = PlayerPrefs.GetFloat("SavedFear", 350f);
        dependence = PlayerPrefs.GetFloat("SavedDepedence", 200f);
        sanity = PlayerPrefs.GetFloat("SavedSanity", 900f);

        Debug.Log("PetState loaded");
    }

    public void SaveState()
    {
        PlayerPrefs.SetFloat("SavedHappy", happy);
        PlayerPrefs.SetFloat("SavedEnergy", energy);
        PlayerPrefs.SetFloat("SavedHunger", hunger);
        PlayerPrefs.SetFloat("SavedClean", clean);
        PlayerPrefs.SetInt("SavedSleep", isSleeping ? 1 : 0);

        PlayerPrefs.SetFloat("SavedTrust", trust);
        PlayerPrefs.SetFloat("SavedFear", fear);
        PlayerPrefs.SetFloat("SavedDepedence", dependence);
        PlayerPrefs.SetFloat("SavedSanity", sanity);

        PlayerPrefs.SetString("LastSaveTime", DateTime.Now.ToString());
        PlayerPrefs.Save();

        Debug.Log("PetState saved");
    }

    public void SpendEnergy (float amount)
    {
        if (energy < amount)
        {
            Debug.Log($"Spending {amount} of energy when it has {energy}");
            trust -= 20f;
            sanity -=15f;
            fear += 10f;
            dependence -= 5f;
        }
        energy -= amount;

        ClampStats();
        SaveState();
    }

    //runs when the app is swiped or closed
    private void OnApplicationQuit()
    {
        SaveState();
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ScheduleOfflineNotifications(this);
        }
    } 

    //runs when app is minimized
    private void OnApplicationPause(bool spause) 
    {
        if (spause)
        {
            SaveState();
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ScheduleOfflineNotifications(this);
            }
        }
    }

    private void OnDestroy()
    {
        SaveState();
    }
    
}
