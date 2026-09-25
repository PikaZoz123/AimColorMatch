using UnityEngine;

[CreateAssetMenu]
public class ReconfiguratorBalanceProfileSO : ScriptableObject
{
    [Range(0f, 100f)] public float oneMoveOpportunityChance = 50f;
    [Range(0f, 100f)] public float twoMoveOpportunityChance = 30f;
    [Range(0f, 100f)] public float threeMoveOpportunityChance = 15f;
    [Range(0f, 100f)] public float messyStateChance = 5f;
}