using System;
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

    [SerializeField]
    private List<Interval> intervals;


    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (!beatTrack.isPlaying)
        {
            return;
        }

        foreach (Interval interval in intervals)
        {
            float sampledTime = (beatTrack.timeSamples / (beatTrack.clip.frequency * interval.GetBeatLength(bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }

    public void RegisterBeatFunction(UnityAction triggerFunction, float subdivisions = 1)
    {
        Interval newInterval = new Interval();
        newInterval.subdivisions = subdivisions;
        UnityEvent trigger = new UnityEvent();
        trigger.AddListener(triggerFunction);
        newInterval.trigger = trigger;

        intervals.Add(newInterval);
    }

    public static BeatManager GetInstance()
    {
        return instance;
    }
}

[Serializable]
public class Interval
{
    public float subdivisions;
    [SerializeField]
    public UnityEvent trigger;
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