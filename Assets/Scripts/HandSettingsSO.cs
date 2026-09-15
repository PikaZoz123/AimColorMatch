using UnityEngine;

[CreateAssetMenu]
public class HandSettingsSO : ScriptableObject
{
    public int handSize = 3;
    public int initialStateDrivenCount = 2;
    [Range(0, 1f)] public float replacementStateDrivenChance = 0.70f;
}