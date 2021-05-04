﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// I use SerializeField instead of a public variable
// Because I only want to show the variable in the inspector
// and not access it from outside
public class Timer : MonoBehaviour
{
    // Observer pattern, method signature
    // Changed the type to IEnumerator so
    // I can wait for functions to finish before resetting the timer
    public delegate void OnTimeUp();

    // Observer pattern, the event I subscribe to
    public static event OnTimeUp OnTimeUpEvent;

    // The max time for the timer to go off
    [Tooltip("The total time needed for the slider to reach the value of 0")]
    public float totalTime;

    // How much time is left?
    private float currentTime;

    // A property to control the current time
    public float CurrentTime
    {
        // I return the local private variable
        get { return currentTime; }

        // I don't want negative time values to be accidentally set by other scripts
        // Nor I want high values that can break the system
        set
        {
            if (value >= 0)
                currentTime = value;
            if (value > totalTime)
                currentTime = totalTime;
        }
    }

    public bool StopTimer { get; set; }

    // Of course I need a reference to the TimerSlider object
    [Tooltip("The slider game object to act as a timer")]
    [SerializeField] private Slider timerSlider;

    void Awake()
    {
        // I make sure only one Timer object can exist
        // There can be only one
        var instance = FindObjectOfType<Timer>();
        if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        // I start at full time
        currentTime = timerSlider.maxValue = totalTime;
        PlayerHealthSystem.OnPlayerDeathEvent += () => { currentTime = totalTime; };
    }

    void Update()
    {
        if (StopTimer) return;
        // I reduce the time each second
        currentTime -= Time.deltaTime;

        // I clamp the current time, making sure I don't go too high or
        // go too low
        currentTime = Mathf.Clamp(currentTime, 0, totalTime);


        /* 
         * Finally I update the slider's value
         * You may be wondering, why not modifying the slider's timer directly?
         * that is because Im may want to do treatment:
         * change, add, reduce from the current time
         * and accessing the slider each time is inefficient
         */
        timerSlider.value = currentTime;


        // Observer pattern call
        if (currentTime <= 0 && OnTimeUpEvent != null)
            OnTimeUpEvent?.Invoke();
    }
}
