using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JakeTCRAGPPrototype.Controller.Guitar.Sample
{
    public class Guitar_Controller_Sample : MonoBehaviour
    {
        [SerializeField] Guitar_Controller Guiter_Ctrl;
        [SerializeField] SwingLine Edge1;
        [SerializeField] SwingLine Edge2;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Guiter_Ctrl.TriggerSwinging())
                {
                    Debug.Log("Start swinging");
                }
                else
                {
                    Debug.Log("Cannot swing");
                }
            }

            if(Guiter_Ctrl.GetSwingTrailEdges(out SwingLine tmp1, out SwingLine tmp2))
            {
                Edge1 = tmp1;
                Edge2 = tmp2;
            }
            else
            {
                Debug.Log("Failed getting the edges");
            }
        }
    }

}
