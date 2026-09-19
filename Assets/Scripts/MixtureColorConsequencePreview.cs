using UnityEngine;

public class MixtureColorConsequencePreview : ConsequencePreview
{
    [SerializeField] MixtureColor mixtureColor;
    MixtureColorPreviewVisual mixtureColorPreview;

    public ColorItemID ColorID { get; set; }

    public override void HidePreview()
    {
        if (mixtureColorPreview != null)
        {
            Debug.Log($"Preview Object: {mixtureColorPreview.name} destroyed!");
            mixtureColorPreview.Deactivate();
            mixtureColor.ResetCurrentSize();
        }
    }

    public override void PreviewConsequence(ConsequenceItemSO consequence, ColorManager colorManager)
    {
        MixtureColorSO mixtureColorSo = null;
        float previewSize = 0;
        if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.RemoveMixtureColor } removeMixtureColor)
        {
            previewSize = 0; // shows the mixture color removed
        }
        else if (consequence is AffectMixtureColorSO affectMixtureColor)
        {
            previewSize = mixtureColor.GetCurrentSize() + affectMixtureColor.affectValue;
        }

        SpawnPreviewVisual(previewSize);
    }

    void SpawnPreviewVisual(float previewSize)
    {
        mixtureColorPreview = mixtureColor.GetComponent<MixtureColorPreviewVisual>();
        mixtureColorPreview.SetData(mixtureColor.GetCurrentSize(), mixtureColor.GetMaxCapacity(), mixtureColor.GetBarWidth(), mixtureColor.GetColor());
        mixtureColorPreview.SetPreviewSize(previewSize);
        mixtureColorPreview.Animate();
    }
}