using UnityEngine;

public class AnchorsManager : ConsequenceHandler
{
    [SerializeField] Anchor[] anchors;
    int chosenRandomAnchor = -1;

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


    void GenerateGameplayColorAtRandomAnchor(ColorItemID colorItemID)
    {
        var a = anchors[chosenRandomAnchor];

        a.PlaceNewGeneratedColorObject(colorManager.GetOneGameplayColorData(colorItemID));
        Debug.Log($"New Gameplay Color Generated: {colorItemID} at Anchor: {a.name}");
    }


    protected override void OnConsequencesHappened(ConsequenceItemSO[] consequencesArray)
    {
        foreach (var consequence in consequencesArray)
        {
            if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.GenerateGameplayColor } changeColorExistence) // checks if generate new color existence consequence
            {
                GenerateGameplayColorAtRandomAnchor(changeColorExistence.colorItemID);
            }
        }
    }

    public void SetRandomAnchorIndex(int previewedAnchorIndex)
    {
        chosenRandomAnchor = previewedAnchorIndex;
    }
}