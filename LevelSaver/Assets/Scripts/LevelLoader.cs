using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public SceneBuilder _sceneBuilder;

    public LevelData CurrentLevelData
    {
        get
        {
            return _currentLevelData;
        }

        set
        {
            if (_currentLevelData != value)
            {
                _currentLevelData = value;
                _sceneBuilder.BuildScene(_currentLevelData);
            }
        }
    }
    private LevelData _currentLevelData;

    public void Start()
    {
        LoadLevel("Sample");
    }

    private List<string> GetLevelNames()
    {
        List<string> levelNames = new List<string>();

        string partialName = string.Empty;

        DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Application.dataPath + "/Resources");
        FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + partialName + "*.txt");

        foreach (FileSystemInfo foundFile in filesAndDirs)
        {
            string fullName = foundFile.Name;
            levelNames.Add(fullName);
        }

        return levelNames;
    }

    public void LoadLevel(string fileName)
    {
        var dataString = Resources.Load(fileName) as TextAsset;
        CurrentLevelData = JsonUtility.FromJson<LevelData>(dataString.text);
    }

    public string ReadDataFromText(string path)
    {
        string data = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    data = reader.ReadToEnd();
                }
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }

        return data;
    }



}
