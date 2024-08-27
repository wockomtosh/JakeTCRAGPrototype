using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TATKController : MonoBehaviour
{
    [SerializeField] string Tag;

    public void Log(object log)
    {
        Debug.Log(tag + ": " + log);
    }
}
