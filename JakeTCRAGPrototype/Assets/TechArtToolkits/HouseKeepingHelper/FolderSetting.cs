using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FolderSetting : ScriptableObject
{
    public string FolderName;
    public bool Required;
    public bool RequireHouseKeepingSetting;
    public List<FileSetting> RegisteredFileSettings;
    public List<string> invalidFiles = new List<string>();

    [MenuItem("Assets/Create/TATK/HouseKeepingHelper/FolderSetting")]
    public static void CreateMyAsset()
    {
        FolderSetting asset = CreateInstance<FolderSetting>();
        ProjectWindowUtil.CreateAsset(asset, "NewFolderSetting.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public void OrganizeFolder(HouseKeepingSetting currentHouseKeeping)
    {
        invalidFiles.Clear();
        string currentPath = currentHouseKeeping.Location + FolderName;
        List<string> currentFiles = FIO_Utils.GetAllFilesUnder(currentPath).ToList();
        if(FolderName == "Materials")
        {
            Debug.Log("All Files under " + currentPath + " :");

            foreach(string file in currentFiles)
            {
                Debug.Log(file);
            }

        }

        foreach (FileSetting fileSetting in RegisteredFileSettings)
        {
            List<string> inspectingFiles = FIO_Utils.GetFilesWithSubFileName(currentFiles, fileSetting.FileSubName);
            
            Debug.Log("InspectingFileType: " + fileSetting.FileSubName);
            foreach (string inspectingFile in inspectingFiles)
            {
                Debug.Log(inspectingFile);
            }
            for(int i = 0; i < inspectingFiles.Count; i++)
            {
                if(!FitConvention(FIO_Utils.GetFileNameFromPath(inspectingFiles[i]), fileSetting.Convention, currentHouseKeeping.ObjectName))
                {
                    invalidFiles.Add(inspectingFiles[i]);
                    currentFiles.Remove(inspectingFiles[i]);
                }
            }
        }
        foreach (string file in currentFiles) 
        {
            invalidFiles.Add(file);
        }
        currentFiles.Clear();
    }

    bool FitConvention(string fileName, string convention, string objectName)
    {
        string[] conventionWords = convention.Split('_');
        string[] fileNameWords = convention.Split('_');

        if(conventionWords.Length != fileNameWords.Length)
        {
            return false;
        }

        for(int i = 0; i < conventionWords.Length; i++)
        {
            if (conventionWords[i] == "Name")
            {
                if (fileNameWords[i] != objectName)
                {
                    return false;
                }
            }
            else if (conventionWords[i] == "DontCare")
            {
                continue;
            }
            else
            {
                if(conventionWords[i] != fileNameWords[i])
                {
                    return false;
                }
            }
        }
        return true;
    }
}

[System.Serializable]
public class FileSetting
{
    public string Convention;
    public string FileSubName;
    public bool Required;
}
