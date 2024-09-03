using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FolderSetting : ScriptableObject
{
    public string FolderName;
    public bool Required;
    public bool RequireHouseKeepingSetting;
    public List<FileSetting> RegisteredFileSettings;

    [MenuItem("Assets/Create/TATK/HouseKeepingHelper/FolderSetting")]
    public static void CreateMyAsset()
    {
        FolderSetting asset = ScriptableObject.CreateInstance<FolderSetting>();
        ProjectWindowUtil.CreateAsset(asset, "NewFolderSetting.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public void CleanFolder(ref HouseKeepingSetting currentHouseKeeping)
    {
        string currentPath = currentHouseKeeping.Location + FolderName;

    }
}

[System.Serializable]
public class FileSetting
{
    public string Convention;
    public string FileSubName;
    public bool Required;
}
