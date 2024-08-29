using UnityEngine;
using UnityEditor;
using System.Collections;

class HouseKeepingHelper_Window : EditorWindow
{
    HouseKeepingSetting Setting;

    void OnGUI()
    {
        Setting = (HouseKeepingSetting)EditorGUILayout.ObjectField(Setting, typeof(HouseKeepingSetting));
        if (GUILayout.Button("HouseKeeping"))
        {
            Setting.OrganizeObjectFolders();
        }
    }















    [MenuItem("TATK/HouseKeepingHelper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(HouseKeepingHelper_Window));
    }
}