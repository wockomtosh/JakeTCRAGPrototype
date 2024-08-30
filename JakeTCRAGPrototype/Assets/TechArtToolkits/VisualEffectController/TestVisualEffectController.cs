using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TATK.VFX;


public class TestVisualEffectController : VisualEffectController
{
    private void OnEnable()
    {
        Init();
    }

    float returnData;
    void Start()
    {
        foo(ref returnData);
        if (foo(ref returnData) == TATKResult.Success)
        {

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryStartVisualEffect();
        }
    }

    public TATKResult PlayEffect()
    {
        if (IsPlaying)
        {
            Log("Is already playing");
            return TATKResult.Warning;
        }
        StartCoroutine(PlayingEffect());
        return TATKResult.Success;
    }

    bool IsPlaying;
    IEnumerator PlayingEffect()
    {
        IsPlaying = true;
        YieldInstruction wait  = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
        }
        IsPlaying = false;
    }

    public TATKResult foo(ref float data1)
    {
        bool failed = false;
        if (failed)
        {
            data1 = 1.0f;
            return TATKResult.Error_InvalidOutput;
        }
        data1 = 0.0f;
        return TATKResult.Success;
    }
}
