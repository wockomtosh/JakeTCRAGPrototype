using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HouseKeepingSetting : ScriptableObject
{
    public List<string> ObjectNames = new List<string>();
    public List<string> SpecialFolderNames = new List<string>();



















    [MenuItem("Assets/Create/TATK/HouseKeepingSetting")]
    public static void CreateMyAsset()
    {
        HouseKeepingSetting asset = ScriptableObject.CreateInstance<HouseKeepingSetting>();

        ProjectWindowUtil.CreateAsset(asset, "NewHouseKeepingSetting.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    public List<string> allDirectories = new List<string>();
    public List<string> neededDirectories = new List<string>();
    public List<string> invalidDirectories = new List<string>();
    public List<string> missingDirectories = new List<string>();

    public void OrganizeObjectFolders()
    {
        allDirectories = GetAllDirectoriesUnder().ToList();

        #region Get Needed Directories
        for (int i = 0; i < ObjectNames.Count; i++)
        {
            neededDirectories.Add(Application.dataPath + "/" + ObjectNames[i]);
        }

        for (int i = 0; i < SpecialFolderNames.Count; i++)
        {
            neededDirectories.Add(Application.dataPath + "/" + SpecialFolderNames[i]);
        }
        #endregion

        #region Check Invalid Directories
        for (int i = 0; i < allDirectories.Count; i++)
        {
            if (!IsValidDirectory(neededDirectories.ToArray(), allDirectories[i]))
            {
                invalidDirectories.Add(allDirectories[i]);
            }
        }
        #endregion

        #region Check Missing Directories
        for (int i = 0; i < ObjectNames.Count; i++)
        {
            if (!DirectoryExist(ObjectNames[i]))
            {
                missingDirectories.Add(Application.dataPath + "/" + ObjectNames[i]);
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
        string[] tmp = Directory.GetDirectories(Application.dataPath + "");
        for(int i = 0; i < tmp.Length; i++)
        {
            tmp[i] = SlashConvert(tmp[i]);
        }
        return tmp;
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

    public string SlashConvert(string input)
    {
        return input.Replace(@"\", "/");
    }
}
