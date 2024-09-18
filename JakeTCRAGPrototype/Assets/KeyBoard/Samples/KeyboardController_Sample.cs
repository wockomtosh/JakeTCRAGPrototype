using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController_Sample : MonoBehaviour
{
    [SerializeField] Keyboard_Controller KeyboardCtrol;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KeyboardCtrol.TriggerAttack();
        }
    }
}
