using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float countdownTime = 90f; // 1:30 (90 seconds)

    private float currentTime;
    private bool isScaling = false;

    private void Start()
    {
        currentTime = countdownTime;
        UpdateTimerText();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
            CheckScaling();
        }
        else
        {
            currentTime = 0;
            Debug.Log("Timer Ended!");
        }
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.Ceil(currentTime));
        timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    private void CheckScaling()
    {
        if (currentTime < 60 && !isScaling)
        {
            isScaling = true;
        }

        if (isScaling && Mathf.FloorToInt(currentTime) % 1 == 0)
        {
            //Animation
        }
    }
}
