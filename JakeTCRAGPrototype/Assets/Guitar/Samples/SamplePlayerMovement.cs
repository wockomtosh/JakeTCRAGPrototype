using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerMovement : MonoBehaviour
{
    public Vector2 Speed = Vector2.one;

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * Time.deltaTime * Speed.x;
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * Speed.y;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}
