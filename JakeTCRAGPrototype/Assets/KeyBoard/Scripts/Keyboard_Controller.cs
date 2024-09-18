using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Keyboard_Controller : MonoBehaviour
{
    [SerializeField] SoundWaveController SoundWaveCtrl;
    [SerializeField] VisualEffect PS_MusicNodes;
    [SerializeField] Animator KeyboardAmt;

    /// <summary>
    /// Try trigger the attack
    /// </summary>
    /// <returns>If successed</returns>
    public bool TriggerAttack()
    {
        if (!SoundWaveCtrl.SendSoundWave())
        {
            return false;
        }
        KeyboardAmt.SetTrigger("AttackTrigger");

        return true;
    }


    /// <summary>
    /// Get boundingbox of current attack. This function is useful to do a demage check. This bounding box does not consider the height, and is under local space.
    /// </summary>
    /// <param name="begin">The begin point of the boundingbox</param>
    /// <param name="end">The end point of the boundingbox</param>
    /// <returns>If successed</returns>
    public bool GetCurrentAttackBoundingBox(out Vector2 begin, out Vector2 end)
    {
        Vector2 xBeginEnd = Vector2.zero;
        Vector2 yBeginEnd = Vector2.zero;

        xBeginEnd.x = -SoundWaveCtrl.GetWorldScale().x / 2;
        xBeginEnd.y = SoundWaveCtrl.GetWorldScale().x / 2;
        yBeginEnd.x = 0;
        yBeginEnd.y = PS_MusicNodes.transform.localPosition.y;
        begin.x = xBeginEnd.x;
        begin.y = yBeginEnd.x;
        end.x = xBeginEnd.y;
        end.y = yBeginEnd.y;
        return true;
    }
}
