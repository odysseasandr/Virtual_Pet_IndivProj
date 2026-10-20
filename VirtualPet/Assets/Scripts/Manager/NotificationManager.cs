using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using System;
public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeNotifications();
    }

    private void InitializeNotifications()
    {
#if UNITY_ANDROID
        //requires "channel" before sending a notification
        var channel = new AndroidNotificationChannel()
        {
                Id = "pet_status_channel",
                Name = "Pet Status",
                Importance = Importance.High,
                Description = "Notification about your pet's needs"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        //clear notifications before reopening game
        AndroidNotificationCenter.CancelAllNotifications();
#endif
    }

    public void ScheduleOfflineNotifications(PetState state)
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();

        if (state.trust > 800f && state.dependence > 800f)
        {
            SendAndroidNotification("where are you??", "I miss you... please come back", 30);

            SendAndroidNotification("Hi", "Are you there, you wouldn't leave me, would you...?", 120);
        }
        else if (state.sanity < 300f && state.fear > 700f)
        {
            SendAndroidNotification("...", "There is someone here with me...", 60);
        }

        else
        {
            //I SHOULD UPDATE THIS TO *ALL* PHYSICAL NEEDS 
            if (state.hunger > 70f)
            {
                SendAndroidNotification("Pet is Hungry", "Come feed you pet, it is hungry", 10);
            }

        }  
#endif      
    }

#if UNITY_ANDROID
    private void SendAndroidNotification(string title, string text, int fireTimeInMinutes)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddMinutes(fireTimeInMinutes);
        notification.SmallIcon = "default"; //!if i have time change to something better!

        AndroidNotificationCenter.SendNotification(notification, "pet_status_channel");
        Debug.Log($"Scheduled Notification: '{title}' for {fireTimeInMinutes} minutes from now");
    }
#endif
}
