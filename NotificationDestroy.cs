using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationDestroy : MonoBehaviour
{
    public float delayDestroy = 1f;

    void Start()
    {
        StartCoroutine(Delay(delayDestroy));
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
