using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HouseKeepingSetting : ScriptableObject
{
    public List<FolderSetting> RegisteredFolders = new List<FolderSetting>();
    public string Location { get { return GetFileLocation(); } }

    [MenuItem("Assets/Create/TATK/HouseKeepingHelper/HouseKeepingSetting")]
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

    public void HouseKeeping()
    {
        Debug.Log("Location: " + Location);
        allDirectories = GetAllDirectoriesUnder(Location).ToList();

        #region Get Needed Directories
        for (int i = 0; i < RegisteredFolders.Count; i++)
        {
            neededDirectories.Add(Location + RegisteredFolders[i].FolderName);
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
        for (int i = 0; i < RegisteredFolders.Count; i++)
        {
            if (!DirectoryExist(RegisteredFolders[i].FolderName))
            {
                missingDirectories.Add(Location + RegisteredFolders[i]);
            }
        }
        #endregion
    }

    public bool DirectoryExist(string path = "")
    {
        return Directory.Exists(Application.dataPath + "/" + path);
    }

    public string[] GetAllDirectoriesUnder(string path)
    {
        string[] tmp = Directory.GetDirectories(path);
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

    public string GetFileLocation()
    {
        string location = "";
        location = Application.dataPath + AssetDatabase.GetAssetPath(this).Substring(6);
        location = location.Substring(0, location.LastIndexOf("/") + 1);
        return location;
    }

    public string SlashConvert(string input)
    {
        return input.Replace(@"\", "/");
    }
}
