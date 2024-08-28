using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TATKController : MonoBehaviour
{
    [SerializeField] string Tag;

    public void Log(object log)
    {
        Debug.Log(Tag + ": " + log);
    }

    public TATKResult TryFindComponent<T>(out T result) where T : Component
    {
        result = GetComponent<T>();
        if (result == null)
        {
            return TATKResult.Error_InvalidOutput;
        }
        return TATKResult.Success;
    }

}
