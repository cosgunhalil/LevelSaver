using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

#if UNITY_EDITOR
using UnityEditor;

public class LevelEditor : EditorWindow
{

    public static LevelEditor window;
    public string NewLevelName = String.Empty;

    public Vector2 scrollPosition = Vector2.zero;
    private LevelItem[] _itemsToSave;
    private List<string> _savedLevelNames = new List<string>();
    private LevelData _levelData;

    public GameObject RectanglePrefab;
    public GameObject CirclePrefab;

    [MenuItem("Tools/LevelEditor")]
    public static void CreateWindow()
    {
        window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor)); //create a window
        window.title = "Level Editor"; 
    }

    void OnGUI()
    {
        if (window == null)
        {
            CreateWindow();
            _savedLevelNames = new List<string>();
        }


        NewLevelName = GUI.TextField(new Rect(10, 10, position.width, 20), NewLevelName, 25);

        if (GUI.Button(new Rect(10, 40, position.width, 20), "Save"))
        {
            Save(NewLevelName);
        }

        if (GUI.Button(new Rect(10, 70, position.width, 20), "LoadLevels"))
        {
            CreateLevelButtons();
        }

        GUILayout.BeginArea(new Rect(10, 110, position.width, position.height));
        for (int i = 0; i < _savedLevelNames.Count; i++)
        {
            if (GUILayout.Button(_savedLevelNames[i]))
            {
                LoadDataFromJson(_savedLevelNames[i]);
            }
        }
        GUILayout.EndArea();

    }

    public void CreateLevelButtons()
    {
        _savedLevelNames = new List<string>();

        _savedLevelNames = GetLevelNames();

        if (GUI.Button(new Rect(10, 90, position.width, 20), "Level1"))
        {
            Debug.Log("Test is completed");
        }
    }

    private void Save(string levelName)
    {
        var itemsToSave = FindObjectsOfType<LevelItem>();

        string path = Application.dataPath + "/Resources/" + levelName + ".txt";
        var data = SerializeMapData(itemsToSave);

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {

                writer.Write(data);
            }

        }

        AssetDatabase.Refresh();

    }

    private string SerializeMapData(LevelItem[] itemsToSave)
    {
        LevelData levelData = new LevelData();

        SetCameraRectSizes(levelData);

        foreach (var item in itemsToSave)
        {
            LevelItemData levelItemData = new LevelItemData();
            levelItemData.Type = item.Type;
            levelItemData.Size = item.transform.localScale;
            levelItemData.Position = item.transform.position;
            levelItemData.Rotation = item.transform.eulerAngles;
            levelData.LevelItems.Add(levelItemData);
        }

        var data = JsonUtility.ToJson(levelData);

        return data;
    }

    private void SetCameraRectSizes(LevelData levelData)
    {
        levelData.CameraHeight = 2f * Camera.main.orthographicSize;
        levelData.CameraWidth = levelData.CameraHeight * Camera.main.aspect;
    }

    public void LoadDataFromJson(string fileName)
    {
        string path = Application.dataPath + "/Resources/" + fileName;
        var data = ReadDataFromText(path);
        var levelData = JsonUtility.FromJson<LevelData>(data);
        LoadScene(levelData);
    }

    private void LoadScene(LevelData levelData)
    {
        ClearScene();


        foreach (var levelItem in levelData.LevelItems)
        {
            var levelItemObject = GenerateLevelItem(levelItem.Type);
            var levelItemObjectData = levelItemObject.GetComponent<LevelItem>();
            levelItemObjectData.transform.localScale = levelItem.Size;
            levelItemObjectData.transform.position = levelItem.Position;
            levelItemObjectData.transform.eulerAngles = levelItem.Rotation;

        }
    }

    private GameObject GenerateLevelItem(ELevelItemType type)
    {
        GameObject source;
        switch (type)
        {
            case ELevelItemType.rectangle:
                source = RectanglePrefab;
                break;
            case ELevelItemType.circle:
                source = CirclePrefab;
                break;
            default:
                source = RectanglePrefab;
                break;
        }

        var createdItem = Instantiate(source) as GameObject;

        return createdItem;
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

    private void ClearScene()
    {
        var levelItems = GameObject.FindObjectsOfType<LevelItem>();

        foreach (var rect in levelItems)
        {
            DestroyImmediate(rect.gameObject);
        }
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
}

#endif

public enum EObjectType
{
    movingSquare
}

[Serializable]
public class LevelData
{
    public List<LevelItemData> LevelItems;

    public float CameraHeight;
    public float CameraWidth;

    public LevelData()
    {
        LevelItems = new List<LevelItemData>();
    }
}

[Serializable]
public class LevelItemData
{
    public ELevelItemType Type;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Size;
}

public enum ELevelItemType
{
    //Add types of gameobjects you have
    rectangle,
    circle
}
