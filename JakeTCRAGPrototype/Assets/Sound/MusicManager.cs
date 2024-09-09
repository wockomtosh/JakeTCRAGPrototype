using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct AdjustableTrack
{
    public AudioSource rhythm;
    public AudioSource lead;
    public Slider slider;
    public PowerUp powerUp;
    public float increaseAmount;
}

public class MusicManager : MonoBehaviour
{
    static MusicManager instance;

    [Header("Background tracks")]
    [SerializeField] AudioSource drums;

    [Header("Adjustable tracks")]
    [SerializeField] AdjustableTrack guitar;
    [SerializeField] AdjustableTrack keyboard;

    [Header("Track Cooldown Props")]
    [Tooltip("A factor by which delta time is divided in order to determine how quickly the rhythm drops in volume. The larger the number the slower the cooldown")]
    [SerializeField] float rhythmCooldownFactor = 20f;

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

        //StartMusic();
    }

    public void StartMusic()
    {
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

        foreach (EnemyController enemy in GameObject.FindObjectsOfType<EnemyController>())
        {
            enemy.Play();
        }

    }

    //TODO: Have some sort of fade-in/out for the tracks rather than just mute/unmute?
    void Update()
    {
        //In here we want to manage the lead meters and also reduce volume over time for instruments that fall out of use
        UpdateTrack(ref guitar);
        UpdateTrack(ref keyboard);
    }

    void UpdateTrack(ref AdjustableTrack track)
    {
        if (track.rhythm.volume > .5f)
        {
            track.rhythm.volume -= Time.deltaTime / rhythmCooldownFactor;
        }

        if (track.rhythm.volume <= .5f)
        {
            track.lead.mute = true;
            CharacterControls.GetInstance().RemovePowerUp(track.powerUp);
        }

        track.slider.value = (track.rhythm.volume - .5f) * 2;
    }

    public void IncreaseGuitar()
    {
        IncreaseTrack(ref guitar);
    }

    public void IncreaseKeyboard()
    {
        IncreaseTrack(ref keyboard);
    }

    void IncreaseTrack(ref AdjustableTrack track)
    {
        track.rhythm.volume += track.increaseAmount;
        if (track.rhythm.volume >= 1)
        {
            //TODO: Set the volume here to the base and don't let it increment??
            track.lead.mute = false;
            track.slider.value = 1;
            CharacterControls.GetInstance().SetPowerUp(track.powerUp);
        }
    }

    public static MusicManager GetInstance()
    {
        return instance;
    }
}
