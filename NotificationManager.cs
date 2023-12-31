using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField]
    private Transform notificationPanel;

    [SerializeField]
    private GameObject notificationPrefab;

    [SerializeField]
    private AudioSource notificationSound;

    private void Awake()
    {
        Instance = this;
    }

    public void Notification(string text, float time)
    {
        if(notificationSound != null) 
            notificationSound.Play();

        GameObject obj = Instantiate(notificationPrefab, notificationPanel);
        obj.GetComponent<NotificationDestroy>().delayDestroy = time;
        obj.transform.Find("NotificationText").GetComponent<TMP_Text>().text = text;
    }

}
