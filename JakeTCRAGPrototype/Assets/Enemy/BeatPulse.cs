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
    private float beatScaleAmount = 1.3f;
    private Vector3 beatScale;
    private Vector3 baseScale;
    [SerializeField]
    private float scaleDownTime = .1f;
    private float elapsedTime = 0;

    void Start()
    {
        beatAction += StartBeatPulse;
        baseScale = transform.localScale;
        beatScale = baseScale * beatScaleAmount;
    }

    public void Play()
    {
        BeatManager.GetInstance().RegisterBeatFunction(beatAction, beatSubdivision);
    }

    void Update()
    {
        if (transform.localScale.magnitude > baseScale.magnitude)
        {
            float timeRatio = elapsedTime / scaleDownTime;
            transform.localScale = Vector3.Lerp(beatScale, baseScale, timeRatio);
            elapsedTime += Time.deltaTime;
        }
        else
        {
            transform.localScale = baseScale;
        }
        
    }

    void StartBeatPulse()
    {
        transform.localScale = beatScale;
        elapsedTime = 0;
    }
}
