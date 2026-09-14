using System;
using UnityEngine;

public class AnchorsManager : ConsequenceHandler
{
    [SerializeField] Shooter shooter;
    [SerializeField] Anchor[] anchors;
    Anchor chosenAnchor => shooter.GetTargetedAnchor();

    void Start()
    {
        FillAnchors();
    }

    void FillAnchors()
    {
        var gameplayColorDataArray = colorManager.GetGameplayColorsData();

        for (var i = 0; i < anchors.Length; i++)
        {
            var a = anchors[i];

            if (a.PlaceNewColorObject(gameplayColorDataArray[i], out var newColorObject))
            {
                gameplayObjectsEventHandler.ListenToGameplayObjectEvents(newColorObject);
            }
        }
    }


    protected override void OnConsequencesHappened(ConsequenceItemSO[] consequencesArray)
    {
        foreach (var consequence in consequencesArray)
        {
            if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.GenerateGameplayColor } changeColorExistence) // checks if generate new color existence consequence
            {
                var colorItemID = changeColorExistence.colorItemID;

                var newColorObject = chosenAnchor.PlaceNewGeneratedColorObject(colorManager.GetOneGameplayColorData(colorItemID));
                gameplayObjectsEventHandler.ListenToGameplayObjectEvents(newColorObject);

                Debug.Log($"New Gameplay Color Generated: {colorItemID} at Anchor: {chosenAnchor.name}");
            }
        }
    }

    public int GetActiveAnchorIndex()
    {
        return Array.IndexOf(anchors, chosenAnchor);
    }
}