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
            GameObject levelItem = _objectGenerator.GenerateLevelItem(levelItemData.Type);
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
}
