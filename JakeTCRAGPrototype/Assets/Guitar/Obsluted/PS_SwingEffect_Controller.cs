using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PS_SwingEffect_Controller : MonoBehaviour
{

    [SerializeField]VisualEffect PS_SwingEffect;
    void Start()
    {
        
    }

    void Update()
    {
        PS_SwingEffect.SetVector3("Pos", transform.position);
    }
}
