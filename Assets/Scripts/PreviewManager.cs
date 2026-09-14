using UnityEngine;

public abstract class PreviewManager : MonoBehaviour
{
    [SerializeField] protected ColorManager colorManager;
    [SerializeField] protected GameplayObjectsEventHandler gameplayObjectsEventHandler;
    protected ConsequencePreview[] previewsArray;

    protected virtual void Awake()
    {
        HandleEventSub();
    }


    protected virtual void HandleEventSub()
    {
        gameplayObjectsEventHandler.onConsequencesPreviewed?.AddListener(OnConsequencesPreviewed);
    }

    protected abstract void OnConsequencesPreviewed(ConsequenceItemSO[] consequencesArray, bool showPreview);
}
