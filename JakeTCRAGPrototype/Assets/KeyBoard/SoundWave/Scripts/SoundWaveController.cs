using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR;

public class SoundWaveController : MonoBehaviour
{
    [Header("DevelopSetting")]
    [SerializeField][Range(0.0f, 1.0f)] float StartFadingXThreshold = 0.5f;
    [SerializeField][Range(0.0f, 1.0f)] float PSStopThreshold = 0.5f;
    [SerializeField] AnimationCurve ScaleCarve;
    [SerializeField][Range(0.01f, 10.0f)] float WaveSpeed = 0.1f;

    [Header("Debug")]
    [SerializeField] MeshRenderer MeshR;
    [SerializeField] VisualEffect PS_MusicNodes;
    [SerializeField] public bool IsSendingWave = false;
    [SerializeField] float ForwardDis = 0;
    [SerializeField] float OriginalFloatingSpeed;
    [SerializeField] float OriginalWaveScale;

    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        OriginalFloatingSpeed = MeshR.material.GetFloat("_FlowSpeed");
        OriginalWaveScale = MeshR.material.GetFloat("_WaveScale");
        MeshR.material.SetVector("_BeginEnd", Vector2.zero);
        PS_MusicNodes.Stop();
    }

    public Vector3 GetWorldScale()
    {
        return transform.lossyScale;
    }

    public bool SendSoundWave()
    {
        if (IsSendingWave)
        {
            return false;
        }
        StartCoroutine(SendingSoundWave());
        return true;
    }

    IEnumerator SendingSoundWave()
    { 
        YieldInstruction wait = new WaitForEndOfFrame();
        Vector2 currentBeginEnd = new Vector2(0.0f, 0.0f);
        float currentWaveScale = 0.0f;
        float passedTime = 0;
        IsSendingWave = true;
        bool pSIsStopped = false;
        yield return new WaitForSeconds(0.1f);
        PS_MusicNodes.Play();
        while (currentBeginEnd.x < 1 || currentBeginEnd.y < 1)
        {
            currentWaveScale = ScaleCarve.Evaluate(currentBeginEnd.x);
            if (currentBeginEnd.x < 1)
            {
                currentBeginEnd.x += Time.deltaTime * WaveSpeed;
            }

            if (currentBeginEnd.y < 1 && currentBeginEnd.x >= StartFadingXThreshold)
            {
                currentBeginEnd.y += Time.deltaTime * WaveSpeed;

            }
            if (!pSIsStopped)
            {
                if(currentBeginEnd.x >= PSStopThreshold)
                {
                    PS_MusicNodes.Stop();
                }
            }

            ForwardDis = currentBeginEnd.x * MeshR.gameObject.transform.localScale.z;
            Vector3 tmpPos = PS_MusicNodes.transform.localPosition;
            tmpPos.z = ForwardDis + MeshR.transform.localPosition.z;
            PS_MusicNodes.transform.localPosition = tmpPos;
            MeshR.material.SetVector("_BeginEnd", currentBeginEnd);
            MeshR.material.SetFloat("_WaveScale", currentWaveScale * OriginalWaveScale);
            passedTime += Time.deltaTime;
            yield return wait;
        }
 
        IsSendingWave = false;
    }
}
