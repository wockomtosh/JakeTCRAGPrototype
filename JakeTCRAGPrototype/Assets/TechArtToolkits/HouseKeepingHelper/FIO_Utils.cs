using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FIO_Utils : MonoBehaviour
{
    public static bool DirectoryExist(string path = "")
    {
        return Directory.Exists(path);
    }

    public static string[] GetAllDirectoriesUnder(string path)
    {
        if (!DirectoryExist(path))
        {
            return new string[0];
        }
        string[] tmp = Directory.GetDirectories(path);

        for (int i = 0; i < tmp.Length; i++)
        {
            tmp[i] = SlashConvert(tmp[i]);
        }
        return tmp;
    }

    public static string[] GetAllFilesUnder(string path, bool ignorMetaData = true)
    {
        if (!DirectoryExist(path))
        {
            return new string[0];
        }

        string[] tmp = Directory.GetFiles(path);
        for (int i = 0; i < tmp.Length; i++)
        {
            tmp[i] = SlashConvert(tmp[i]);
        }

        if (ignorMetaData)
        {
            List<string> tmpList = tmp.ToList();
            for(int i = 0; i < tmpList.Count; i++)
            {
                Debug.Log(GetFileSubNameFromPath(tmpList[i]));
                if (GetFileSubNameFromPath(tmpList[i]) == "meta")
                {
                    tmpList.RemoveAt(i);
                    i--;
                }
            }
            tmp = tmpList.ToArray();    
        }

        return tmp;
    }

    public static string GetFileNameFromPath(string path)
    {
        string[] tmp = path.Split('/');
        tmp = tmp[tmp.Length - 1].Split('.');
        return tmp[tmp.Length - 1];
    }

    public static string GetFileSubNameFromPath(string path)
    {
        string[] tmp = path.Split('.');
        return tmp[tmp.Length - 1];
    }

    public static List<string> GetFilesWithSubFileName(List<string> allFiles, string subFileName)
    {
        List<string> matchedFiles = new List<string>();

        foreach (string file in allFiles)
        {
            string[] tmp = file.Split(".");
            if (tmp[tmp.Length - 1] == subFileName)
            {
                matchedFiles.Add(file);
            }
        }
        return matchedFiles;
    }

    public static bool IsValidDirectory(string[] allowedDirectories, string path = "")
    {
        bool value = false;
        for (int i = 0; i < allowedDirectories.Length; i++)
        {
            if (allowedDirectories[i] == path)
            {
                value = true;
            }
        }
        return value;
    }


    public static string SlashConvert(string input)
    {
        return input.Replace(@"\", "/");
    }
}
