using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace TATK.VFX
{
    public class VisualEffectController : TATKController
    {
        [SerializeField] VisualEffect VE;

        protected TATKResult Init()
        {
            TATKResult tmpResult;
            tmpResult = TryFindComponent(out VE);
            if (tmpResult != TATKResult.Success)
            {
                return tmpResult;
            }
            VE.Stop();
            return TATKResult.Success;
        }

        protected TATKResult TryStartVisualEffect()
        {
            VE.Play();  
            Log("AAA");
            return TATKResult.Success;
        }
    }

}

