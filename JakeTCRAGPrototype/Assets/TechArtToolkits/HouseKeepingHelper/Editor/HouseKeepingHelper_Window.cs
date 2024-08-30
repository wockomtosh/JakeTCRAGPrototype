using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class HouseKeepingHelper_Window : EditorWindow
{
    HouseKeepingSetting Setting;
    List<string> CurrentSpecialFoldersInProject;
    int CurrentSelectedObjectIndex = 0;
    void OnGUI()
    {
        Setting = (HouseKeepingSetting)EditorGUILayout.ObjectField(Setting, typeof(HouseKeepingSetting));

        
        //EditorGUILayout.Popup(0, Setting.RegisteredFolders.ToArray());
        //EditorGUILayout.LabelField("Object Settings");

        if (GUILayout.Button("HouseKeeping"))
        {
            Setting.HouseKeeping();
        }
    }















    [MenuItem("TATK/HouseKeepingHelper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(HouseKeepingHelper_Window));
    }
}