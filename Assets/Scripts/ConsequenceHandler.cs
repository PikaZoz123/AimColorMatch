using UnityEngine;

public abstract class ConsequenceHandler : MonoBehaviour
{
    [SerializeField] protected ColorManager colorManager;
    [SerializeField] protected GameplayObjectsEventHandler gameplayObjectsEventHandler;

    protected virtual void Awake()
    {
        HandleEventSub();
    }
    protected virtual void HandleEventSub()
    {
        gameplayObjectsEventHandler.onConsequencesHappened?.AddListener(OnConsequencesHappened);
    }

    protected abstract void OnConsequencesHappened(ConsequenceItemSO[] consequencesArray);
}
