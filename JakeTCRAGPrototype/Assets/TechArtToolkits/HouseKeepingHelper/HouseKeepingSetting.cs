using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HouseKeepingSetting : ScriptableObject
{
    public string ObjectName;
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
    public List<string> validDirectories = new List<string>();
    public List<string> missingDirectories = new List<string>();

    public void HouseKeeping()
    {
        allDirectories.Clear();
        neededDirectories.Clear();  
        missingDirectories.Clear(); 
        invalidDirectories.Clear();
        validDirectories.Clear();

        Debug.Log("Location: " + Location);
        allDirectories = FIO_Utils.GetAllDirectoriesUnder(Location).ToList();

        #region Get Needed Directories
        for (int i = 0; i < RegisteredFolders.Count; i++)
        {
            neededDirectories.Add(Location + RegisteredFolders[i].FolderName);
        }
        #endregion
        for (int i = 0; i < allDirectories.Count; i++)
        {
            #region Check Invalid Directories
            if (!FIO_Utils.IsValidDirectory(neededDirectories.ToArray(), allDirectories[i]))
            {
                invalidDirectories.Add(allDirectories[i]);
            }
            #endregion
            #region Check Valid Directories
            else
            {
                validDirectories.Add(allDirectories[i]);
            }
            #endregion
        }

        #region Check Missing Directories
        for (int i = 0; i < RegisteredFolders.Count; i++)
        {
            if (!FIO_Utils.DirectoryExist(Location + RegisteredFolders[i].FolderName))
            {
                missingDirectories.Add(Location + RegisteredFolders[i].FolderName);
            }
        }
        #endregion

        foreach(FolderSetting folderSetting in RegisteredFolders)
        {
            folderSetting.OrganizeFolder(this);
        }
    }

   

    string GetFileLocation()
    {
        string location = "";
        location = Application.dataPath + AssetDatabase.GetAssetPath(this).Substring(6);
        location = location.Substring(0, location.LastIndexOf("/") + 1);
        return location;
    }
}
