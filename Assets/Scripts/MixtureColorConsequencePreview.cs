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
            mixtureColorSo = colorManager.GetOneMixtureColorData(removeMixtureColor.colorItemID);
        }
        else if (consequence is AffectMixtureColorSO affectMixtureColor)
        {
            previewSize = mixtureColor.GetCurrentSize() + affectMixtureColor.affectValue;
            mixtureColorSo = colorManager.GetOneMixtureColorData(affectMixtureColor.colorToAffect);
        }

        SpawnPreviewVisual(mixtureColorSo, previewSize);
    }

    void SpawnPreviewVisual(MixtureColorSO mixtureColorSo, float previewSize)
    {
        mixtureColorPreview = mixtureColor.GetComponent<MixtureColorPreviewVisual>();
        mixtureColorPreview.SetData(mixtureColorSo, mixtureColor.GetMaxCapacity(), mixtureColor.GetBarWidth());
        mixtureColorPreview.SetPreviewSize(previewSize);
        mixtureColorPreview.Animate();
    }
}