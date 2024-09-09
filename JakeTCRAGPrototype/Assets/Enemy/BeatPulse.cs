using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatPulse : MonoBehaviour
{
    private UnityAction beatAction;
    [SerializeField]
    private float beatSubdivision = 1;
    [SerializeField]
    private float beatScale = 1.3f;
    private Vector3 baseScale;
    [SerializeField]
    private float scaleDownTime = .1f;

    void Start()
    {
        beatAction += StartBeatPulse;
        baseScale = transform.localScale;
    }

    public void Play()
    {
        BeatManager.GetInstance().RegisterBeatFunction(beatAction, beatSubdivision);
    }

    void Update()
    {
        if (transform.localScale.magnitude > baseScale.magnitude)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseScale, scaleDownTime);
        }
        else
        {
            transform.localScale = baseScale;
        }
        
    }

    void StartBeatPulse()
    {
        transform.localScale *= beatScale;
    }
}
