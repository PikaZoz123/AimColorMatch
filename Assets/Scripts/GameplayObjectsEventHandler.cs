using UnityEngine;
using UnityEngine.Events;

public class GameplayObjectsEventHandler : MonoBehaviour
{
    [HideInInspector] public UnityEvent<ConsequenceItemSO[]> onConsequencesHappened;
    [HideInInspector] public UnityEvent<ConsequenceItemSO[], bool> onConsequencesPreviewed;


    void OnDestroy()
    {
        onConsequencesHappened?.RemoveAllListeners();
        onConsequencesPreviewed?.RemoveAllListeners();
    }

    public void ListenToGameplayObjectEvents(GameplayColorObject newColorObject)
    {
        var consequencesArray = newColorObject.GetData().consequencesArray;

        newColorObject.onDestroyed.AddListener(x => onConsequencesHappened?.Invoke(consequencesArray));

        newColorObject.onTargeted.AddListener((x, targeted) => onConsequencesPreviewed?.Invoke(consequencesArray, targeted));
    }
}