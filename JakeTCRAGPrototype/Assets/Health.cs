using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int hp = 1;
    [HideInInspector]
    public int maxhp;

    [SerializeField]
    private float iTime = 1f;
    private float iTimer = 2f;

    void Start()
    {
        maxhp = hp;
    }

    void Update()
    {
        if (iTimer < iTime)
        {
            iTimer += Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (iTimer > iTime)
        {
            iTimer = 0f;
            Debug.Log($"{gameObject.name} took {damage} damage");
            hp -= damage;
        }
        
        //if (hp <= 0)
        //{
        //    if (transform.parent != null)
        //    {
        //        Destroy(transform.parent.gameObject);
        //    }
        //    Destroy(gameObject);
        //}
    }


}
