using UnityEngine;
using UnityEditor;
using System.Collections;

class HouseKeepingHelper_Window : EditorWindow
{
    [MenuItem("TATK/HouseKeepingHelper")]

    public static void ShowWindow()
    {
        GetWindow(typeof(HouseKeepingHelper_Window));
    }

    void OnGUI()
    {
        if (GUILayout.Button("HouseKeeping"))
        {

        }
    }
}