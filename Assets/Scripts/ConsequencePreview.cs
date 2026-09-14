using UnityEngine;

public abstract class ConsequencePreview : MonoBehaviour
{
    public abstract void PreviewConsequence(ConsequenceItemSO consequence, ColorManager colorManager);
    public abstract void HidePreview();
}
