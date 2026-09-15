using UnityEngine;

public class AnchorPreviewsManager : PreviewManager
{
    [SerializeField] AnchorsManager anchorsManager;
    ConsequencePreview anchorToPreview => previewsArray[anchorsManager.GetActiveAnchorIndex()];


    protected override void Awake()
    {
        base.Awake();
        previewsArray = GetComponentsInChildren<ConsequencePreview>();
    }

    protected override void OnConsequencesPreviewed(ConsequenceItemSO[] consequencesArray, bool showPreview)
    {
        foreach (var consequence in consequencesArray)
        {
            if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.GenerateGameplayColor } changeColorExistence) // checks if generate new color existence consequence
            {
                PreviewGenerateGameplayColorAtRandomAnchor(changeColorExistence, showPreview);
            }
        }
    }

    void PreviewGenerateGameplayColorAtRandomAnchor(ChangeColorExistenceSO changeColorExistence, bool showPreview)
    {
        if (showPreview)
        {
            anchorToPreview.PreviewConsequence(changeColorExistence, colorManager);
        }
        else
        {
            anchorToPreview.HidePreview();
            Debug.Log($"NOT Previewing Generate Gameplay Color {changeColorExistence.colorItemID} at Random Anchor: {anchorToPreview.name}");
        }
    }
}