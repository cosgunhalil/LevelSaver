using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour {

    public GameObject RectanglePrefab;
    public GameObject CirclePrefab;

    public GameObject GenerateLevelItem(ELevelItemType type)
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
                shape = Instantiate(RectanglePrefab) as GameObject;
                break;
        }

        return shape;
    }
}
