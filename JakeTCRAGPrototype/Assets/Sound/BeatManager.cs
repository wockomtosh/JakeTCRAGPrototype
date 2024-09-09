using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    public static BeatManager instance;

    [SerializeField]
    private float bpm;
    [SerializeField]
    private AudioSource beatTrack;

    private Interval[] intervals;


    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void RegisterBeatFunction(UnityEvent trigger, float subdivisions = 1)
    {
        Interval newInterval = new Interval();
    }

    public static BeatManager GetInstance()
    {
        return instance;
    }
}

public class Interval
{
    private float subdivisions;
    private UnityEvent trigger;
    private int lastInterval;

    public float GetBeatLength(float bpm)
    {
        return 60f / (bpm * subdivisions);
    }

    public void CheckForNewInterval(float interval)
    {
        int intervalCheck = Mathf.FloorToInt(interval);
        if (intervalCheck != lastInterval)
        {
            lastInterval = intervalCheck;
            trigger.Invoke();
        }
    }
}