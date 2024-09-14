using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageTrigger : MonoBehaviour
{
    //This is all gross and should be done differently, this was just a bandaid fix to get this up temporarily
    //Also for some reason doesn't affect enemy projectiles (bc both are triggers?), but that's kind of an interesting feature
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by " + other);
        CharacterControls.GetInstance().ApplyDamageToEnemy(other);
    }
}
