using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject RectanglePrefab;
    public GameObject CirclePrefab;

    public void Start()
    {
        LoadLevel("Sample");
    }

    private List<string> GetLevelNames()
    {
        List<string> levelNames = new List<string>();
        //TODO - find saved files and add to levelNames list
        //string partialName = "Level";

        //DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Application.dataPath + "/Resources");
        //FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + partialName + "*.txt");

        //foreach (FileSystemInfo foundFile in filesAndDirs)
        //{
        //    string fullName = foundFile.Name;
        //    levelNames.Add(fullName);
        //}
        return levelNames;
    }

    public void LoadLevel(string fileName)
    {
        var levelData = new LevelData();
        var dataString = Resources.Load(fileName) as TextAsset;

        levelData = JsonUtility.FromJson<LevelData>(dataString.text);

        LoadScene(levelData);
    }

    private void LoadScene(LevelData levelData)
    {
        ClearScene();

        foreach (var levelItemData in levelData.LevelItems)
        {
            GameObject levelItem = GenerateLevelItem(levelItemData.Type);
            var levelItemView = levelItem.GetComponent<LevelItem>();

            levelItemView.transform.localScale = levelItemData.Size;
            levelItemView.transform.position = levelItemData.Position;
            levelItemView.transform.eulerAngles = levelItemData.Rotation;
        }
    }

    private void ClearScene()
    {
        var rectangles = GameObject.FindObjectsOfType<LevelItem>();

        foreach (var rect in rectangles)
        {
            Destroy(rect.gameObject);
        }
    }

    private GameObject GenerateLevelItem(ELevelItemType type)
    {
        GameObject shape;
        switch (type)
        {
            case ELevelItemType.rectangle:
                shape = Instantiate(RectanglePrefab) as GameObject;
                break;
            case ELevelItemType.circle:
                shape = Instantiate(CirclePrefab) as GameObject;
                break;
            default:
                shape  = Instantiate(RectanglePrefab) as GameObject;
                break;
        }

        return shape;
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
