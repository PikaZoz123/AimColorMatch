using UnityEngine;

[CreateAssetMenu]
public class AnchorDataSO : ScriptableObject
{
    public int clusterSize;
    public float clusterRadius;
    public float minObjectRadius;
    public float maxObjectRadius;
    public float sizeBias;
    public float distanceBias;
}