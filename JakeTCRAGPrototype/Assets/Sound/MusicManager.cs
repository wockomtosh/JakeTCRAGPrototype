using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AdjustableTrack
{
    public AudioSource rhythm;
    public AudioSource lead;
    public float leadDuration;
}

public class MusicManager : MonoBehaviour
{
    static MusicManager instance;

    [Header("Background tracks")]
    [SerializeField] AudioSource drums;

    [Header("Adjustable tracks")]
    [SerializeField] AdjustableTrack guitar;
    [SerializeField] AdjustableTrack keyboard;

    [Header("Music props")]
    [SerializeField]
    private float bpm = 120;
    private float beatLength;
    private float beatTimer = 0;
    [SerializeField]
    private int timeSignature = 4;
    private int curBeatNum = 0;

    void Start()
    {
        instance = this;

        //Set music props
        beatLength = 60 / bpm;

        //Set tracks
        drums.Play();
        drums.volume = .5f;

        guitar.rhythm.Play();
        guitar.rhythm.volume = .5f;
        guitar.lead.Play();
        guitar.lead.volume = .5f;
        guitar.lead.mute = true;

        keyboard.rhythm.Play();
        keyboard.rhythm.volume = .5f;
        keyboard.lead.Play();
        keyboard.lead.volume = .5f;
        keyboard.lead.mute = true;
    }

    //TODO: Have some sort of fade-in/out for the tracks rather than just mute/unmute?
    void Update()
    {
        //In here we want to manage the lead meters and also reduce volume over time for instruments that fall out of use
    }

    //TODO: make these one function and use an enum to choose the instrument?
    public void IncreaseGuitar()
    {
        guitar.rhythm.volume += .1f;
        if (guitar.rhythm.volume >= 1)
        {
            guitar.lead.mute = false;
        }
    }

    public void IncreaseKeyboard()
    {
        keyboard.rhythm.volume += .1f;
        if (keyboard.rhythm.volume >= 1)
        {
            keyboard.lead.mute = false;
        }
    }

    public static MusicManager GetInstance()
    {
        return instance;
    }
}
