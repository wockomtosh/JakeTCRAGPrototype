using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HouseKeepingSetting : ScriptableObject
{
    List<string> ObjectFolderNames = new List<string>();



















    [MenuItem("Assets/Create/TATK/HouseKeepingSetting")]
    public static void CreateMyAsset()
    {
        HouseKeepingSetting asset = ScriptableObject.CreateInstance<HouseKeepingSetting>();

        ProjectWindowUtil.CreateAsset(asset, "NewHouseKeepingSetting.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    public void OrganizeObjectFolders()
    {
        List<string> allDirectories = new List<string>();
        List<string> allowedDirectories = new List<string>();
        List<string> invalidDirectories = new List<string>();

        allDirectories = GetAllDirectoriesUnder().ToList();

        #region Check Invalid Directories
        for(int i = 0; i < allDirectories.Count; i++)
        {
            if (!IsValidDirectory(allowedDirectories.ToArray(), allDirectories[i]))
            {
                invalidDirectories.Add(allDirectories[i]);
            }
        }
        #endregion

        #region Check Missing Directories
        for (int i = 0; i < ObjectFolderNames.Count; i++)
        {
            if (!DirectoryExist(ObjectFolderNames[i]))
            {

            }
        }
        #endregion

    }

    public bool DirectoryExist(string path = "")
    {
        return Directory.Exists(Application.dataPath + "/" + path);
    }

    public string[] GetAllDirectoriesUnder(string folderName = "")
    {
        return Directory.GetDirectories(Application.dataPath + "");
    }
    
    public bool IsValidDirectory(string[] allowedDirectories, string path = "")
    {
        bool value = false;
        for(int i = 0; i < allowedDirectories.Length; i++)
        {
            if (allowedDirectories[i] == path)
            {
                value = true;
            }
        }
        return value;
    }
}
