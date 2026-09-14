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
            Debug.Log($"Preview Object: {activePreviewObject.name} destroyed!");
            Destroy(activePreviewObject.gameObject);
        }
    }

    public override void PreviewConsequence(ConsequenceItemSO consequence, ColorManager colorManager)
    {
        if (consequence is not ChangeColorExistenceSO changeColorExistence || changeColorExistence.colorExistenceID is not ColorExistenceID.GenerateGameplayColor)
        {
            return;
        }


        if (anchor.TryGetNextClusterPoint(out var point))
        {
            activePreviewObject = Instantiate(gameplayColorObjectPreviewPrefab, point.Value.GetWorldPosition(transform), Quaternion.identity);
            activePreviewObject.transform.SetParent(transform);

            GameplayColorSO colorData = colorManager.GetOneGameplayColorData(changeColorExistence.colorItemID);

            activePreviewObject.SetColor(colorData.colorItemSO.colorValue);
            activePreviewObject.name = $"{changeColorExistence.colorItemID}_Preview";

            activePreviewObject.transform.localScale = 2f * point.Value.Radius * Vector3.one;

            Debug.Log($"Previewing Generate Gameplay Color {changeColorExistence.colorItemID} at Random Anchor: {name}");

        }
    }
}
