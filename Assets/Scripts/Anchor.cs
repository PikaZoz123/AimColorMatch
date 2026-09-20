using System;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    [SerializeField] GameplayColorObject gameplayColorPrefab;
    [SerializeField] List<Point> points;
    readonly List<GameplayColorObject> gameplayColorsObjects = new();
    Point nextSpawnPoint;

    // Point previewClusterPoint;


    public bool PlaceNewColorObject(GameplayColorSO gameplayColorSO, out GameplayColorObject newObject) //spawns the colors at the start
    {
        var availableClusterPoint = points[gameplayColorsObjects.Count];

        newObject = InstantiateObject(gameplayColorSO, availableClusterPoint);
        return newObject != null;
    }

    void OnGameplayObjectDestroyed(GameplayColorObject gameplayObject)
    {
        foreach (var p in points)
        {
            if (p.gameplayObject == gameplayObject)
            {
                p.gameplayObject = null;
            }
            else if (nextSpawnPoint == null)
            {
                nextSpawnPoint = p;
            }
        }

        gameplayColorsObjects.Remove(gameplayObject);
    }


    public GameplayColorObject GetColorObject(int index)
    {
        return gameplayColorsObjects[index];
    }

    public void TryGetPreviewClusterPoint(out Point point)
    {
        point = null;
        foreach (var p in points)
        {
            if (p.gameplayObject == null)
            {
                point = p;
                break;
            }
        }
    }

    public GameplayColorObject PlaceNewGeneratedColorObject(GameplayColorSO gameplayColorSO)
    {
        var newGeneratedObject = InstantiateObject(gameplayColorSO, nextSpawnPoint);
        nextSpawnPoint = null;

        return newGeneratedObject;
    }

    GameplayColorObject InstantiateObject(GameplayColorSO gameplayColorSO, Point availableClusterPoint)
    {
        var newObject = Instantiate(gameplayColorPrefab, availableClusterPoint.transform.position, Quaternion.identity);
        newObject.transform.SetParent(transform);
        newObject.SetData(gameplayColorSO);

        newObject.name = $"{gameplayColorSO.colorItemSO.colorItemID}";

        gameplayColorsObjects.Add(newObject);
        availableClusterPoint.gameplayObject = newObject;

        newObject.onDestroyed?.AddListener(OnGameplayObjectDestroyed);
        return newObject;
    }


    [Serializable]
    public class Point
    {
        public Transform transform;
        public GameplayColorObject gameplayObject;
    }
}