using UnityEngine;

[CreateAssetMenu]
public class EvaluatorBalanceProfileSO : ScriptableObject
{
    public float flushValue;

    public float totalPressureWeight;
    public float largestMixtureWeight;
    public float smallestMixtureWeight;

    public float deathPenalty;

    public float futureFlushValue;
    public float futureReductionValue;
    public float futureOptionsValue;

    public float randomness;
}