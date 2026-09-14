using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class Anchor : MonoBehaviour
{
    [SerializeField] GameplayColorObject gameplayColorPrefab;
    [SerializeField] AnchorDataSO anchorDataSO;

    [SerializeField] List<GameplayColorObject> gameplayColorsObjects = new();

    SphereCluster cluster;
    int nextClusterPointIndex;
    private int filledSlotsCount;

    int ClusterSize => anchorDataSO.clusterSize;
    float ClusterRadius => anchorDataSO.clusterRadius;
    float MinObjectRadius => anchorDataSO.minObjectRadius;
    float MaxObjectRadius => anchorDataSO.maxObjectRadius;
    float SizeBias => anchorDataSO.sizeBias;
    float DistanceBias => anchorDataSO.distanceBias;



    private void Awake()
    {
        if (cluster == null)
        {
            cluster = new SphereCluster();
        }
        cluster.Generate(ClusterSize, ClusterRadius, MinObjectRadius, MaxObjectRadius, SizeBias, DistanceBias);

        // init list with empty objects.
        for (int i = 0; i < ClusterSize; i++)
        {
            gameplayColorsObjects.Add(null);
        }


    }



    public bool PlaceNewColorObject(GameplayColorSO gameplayColorSO, out GameplayColorObject newObject) // point used for preview
    {
        newObject = InstantiateObject(gameplayColorSO, filledSlotsCount);
        return newObject != null;
    }

    private void OnGameplayObjectDestroyed(GameplayColorObject gameplayObject)
    {
        for (int i = 0; i < gameplayColorsObjects.Count; i++)
        {
            if (ReferenceEquals(gameplayColorsObjects[i], gameplayObject))
            {
                gameplayColorsObjects[i] = null;
                filledSlotsCount--;
                break;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (cluster == null)
        {
            cluster = new SphereCluster();
        }
        if (anchorDataSO == null)
        {
            return;
        }

        if (cluster.Points.Count != ClusterSize)
        {
            cluster.Generate(ClusterSize, ClusterRadius, MinObjectRadius, MaxObjectRadius, SizeBias, DistanceBias);
        }



        foreach (var point in cluster.Points)
        {
            Gizmos.DrawWireSphere(point.GetWorldPosition(transform), point.Radius);
        }

        Gizmos.DrawWireSphere(transform.position, ClusterRadius);
    }

    public GameplayColorObject GetColorObject(int index)
    {
        return gameplayColorsObjects[index];
    }

    public bool TryGetNextClusterPoint(out SphereCluster.PointData? point)
    {
        point = null;

        for (int i = 0; i < gameplayColorsObjects.Count; i++)
        {
            GameplayColorObject o = gameplayColorsObjects[i];
            if (o == null)
            {
                nextClusterPointIndex = i;

                point = cluster.Points[nextClusterPointIndex];
                return true;

            }
        }
        return false;
    }

    public void PlaceNewGeneratedColorObject(GameplayColorSO gameplayColorSO)
    {
        InstantiateObject(gameplayColorSO, nextClusterPointIndex);
    }

    private GameplayColorObject InstantiateObject(GameplayColorSO gameplayColorSO, int currentIndex)
    {
        if (filledSlotsCount >= cluster.Points.Count)
        {
            Debug.Log($"Anchor: {name} is Full");
            return null;
        }
        var availableClusterPoint = cluster.Points[currentIndex];

        var newObject = Instantiate(gameplayColorPrefab, availableClusterPoint.GetWorldPosition(transform), Quaternion.identity);
        newObject.transform.SetParent(transform);
        newObject.SetData(gameplayColorSO);

        newObject.transform.localScale = 2f * availableClusterPoint.Radius * Vector3.one;

        gameplayColorsObjects[currentIndex] = newObject;

        filledSlotsCount++;

        newObject.onDestroyed?.AddListener(OnGameplayObjectDestroyed);
        return newObject;
    }
}
