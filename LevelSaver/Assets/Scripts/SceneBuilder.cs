using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour {

    public ObjectGenerator _objectGenerator;

    public void BuildScene(LevelData levelData)
    {
        ClearScene();

        foreach (var levelItemData in levelData.LevelItems)
        {
            var levelItem = _objectGenerator.GenerateLevelItem(levelItemData.Type).GetComponent<LevelItem>();
            levelItem.Type = levelItemData.Type;
            levelItem.transform.localScale = levelItemData.Size;
            levelItem.transform.position = levelItemData.Position;
            levelItem.transform.eulerAngles = levelItemData.Rotation;
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
}
