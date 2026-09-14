using System;
using UnityEngine;
using UnityEngine.Events;

public class GameplayObjectsEventHandler : MonoBehaviour
{
    [HideInInspector] public UnityEvent<ConsequenceItemSO[]> onConsequencesHappened;
    [HideInInspector] public UnityEvent<ConsequenceItemSO[], bool> onConsequencesPreviewed;

    public void ListenToGameplayObjectEvents(GameplayColorObject newColorObject)
    {
        ConsequenceItemSO[] consequencesArray = newColorObject.GetData().consequencesArray;

        newColorObject.onDestroyed.AddListener((x) => onConsequencesHappened?.Invoke(consequencesArray));

        newColorObject.onTargeted.AddListener((x, targeted) => onConsequencesPreviewed?.Invoke(consequencesArray, targeted));
    }



    private void OnDestroy()
    {
        onConsequencesHappened?.RemoveAllListeners();
        onConsequencesPreviewed?.RemoveAllListeners();
    }

}
