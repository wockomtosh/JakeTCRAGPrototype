using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicVisualEffect))]
[CanEditMultipleObjects]
public class BasicVisualEffect_Editor : Editor
{
    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
    }
}
