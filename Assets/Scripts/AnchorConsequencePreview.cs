using UnityEngine;

public class AnchorConsequencePreview : ConsequencePreview
{
    [SerializeField] Anchor anchor;
    [SerializeField] GameplayColorObjectPreviewVisual gameplayColorObjectPreviewPrefab;
    GameplayColorObjectPreviewVisual activePreviewObject;

    public override void HidePreview()
    {
        if (activePreviewObject != null)
        {
            //Debug.Log($"Preview Object: {activePreviewObject.name} destroyed!");
            Destroy(activePreviewObject.gameObject);
        }
    }

    public override void PreviewConsequence(ConsequenceItemSO consequence, ColorManager colorManager)
    {
        if (consequence is not ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.GenerateGameplayColor } changeColorExistence)
        {
        }


        // anchor.TryGetPreviewClusterPoint(out var point);
        //
        // activePreviewObject = Instantiate(gameplayColorObjectPreviewPrefab, point.transform.position, Quaternion.identity);
        // activePreviewObject.transform.SetParent(transform);
        //
        // var colorData = colorManager.GetOneGameplayColorData(changeColorExistence.colorItemID);
        //
        // activePreviewObject.SetColor(colorData.colorItemSO.colorValue);
        // activePreviewObject.name = $"{changeColorExistence.colorItemID}_Preview";
        //
        // Debug.Log($"Previewing Generate Gameplay Color {changeColorExistence.colorItemID} at Random Anchor: {name}");
    }
}