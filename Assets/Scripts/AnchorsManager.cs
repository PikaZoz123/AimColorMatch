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
        var gameplayColorDataList = colorManager.GetColorsInHand();

        for (var i = 0; i < anchors.Length; i++)
        {
            var a = anchors[i];

            if (a.PlaceNewColorObject(gameplayColorDataList[i], out var newColorObject))
            {
                HookIntoNewGameplayObject(newColorObject, a);
            }
        }
    }

    void HookIntoNewGameplayObject(GameplayColorObject newColorObject, Anchor a)
    {
        gameplayObjectsEventHandler.ListenToGameplayObjectEvents(newColorObject);
        newColorObject.onDestroyed.AddListener(x => ReplaceGameplayObject(a, x));
    }


    protected override void OnConsequencesHappened(ConsequenceItemSO[] consequencesArray)
    {
        foreach (var consequence in consequencesArray)
        {
            if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.GenerateGameplayColor } changeColorExistence) // checks if generate new color existence consequence
            {
                var colorItemID = changeColorExistence.colorItemID;

                var newColorObject = chosenAnchor.PlaceNewGeneratedColorObject(colorManager.GetOneGameplayColorData(colorItemID));

                HookIntoNewGameplayObject(newColorObject, chosenAnchor);

                Debug.Log($"New Gameplay Color Generated: {colorItemID} at Anchor: {chosenAnchor.name}");
            }
        }
    }


    void ReplaceGameplayObject(Anchor a, GameplayColorObject destroyedObj)
    {
        var id = destroyedObj.GetData().colorItemSO.colorItemID;
        var newColorObject = a.PlaceNewGeneratedColorObject(colorManager.GetConsumedCardReplacement(id));

        HookIntoNewGameplayObject(newColorObject, a);
    }

    public int GetActiveAnchorIndex()
    {
        return Array.IndexOf(anchors, chosenAnchor);
    }
}