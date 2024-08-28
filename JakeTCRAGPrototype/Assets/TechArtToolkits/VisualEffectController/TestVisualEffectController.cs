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


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryStartVisualEffect();
        }
    }
}
