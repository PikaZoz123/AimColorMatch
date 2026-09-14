using System.Collections.Generic;
using UnityEngine;

public class MixtureBarPreviewsManager : PreviewManager
{
    [SerializeField] MixtureBar mixtureBar;


    protected override void Awake()
    {
        base.Awake();
        mixtureBar.onColorsInitialized?.AddListener(InitPreviews);
    }

    void InitPreviews(Dictionary<ColorItemID, MixtureColor> colorDict)
    {
        previewsArray = new ConsequencePreview[colorDict.Count];

        var i = 0;
        foreach (var (id, color) in colorDict)
        {
            var mixtureColorConsequencePreview = color.GetComponent<MixtureColorConsequencePreview>();
            mixtureColorConsequencePreview.ColorID = id;

            previewsArray[i] = mixtureColorConsequencePreview;

            i++;
        }
    }


    protected override void OnConsequencesPreviewed(ConsequenceItemSO[] consequencesArray, bool showPreview)
    {
        foreach (var consequence in consequencesArray)
        {
            if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.RemoveMixtureColor } changeColorExistence) // checks if color existence consequence
            {
                PreviewNullifyMixtureColor(changeColorExistence, showPreview);
            }
            else if (consequence is AffectMixtureColorSO affectColor)
            {
                PreviewAffectMixtureColor(affectColor, showPreview);
            }
        }
    }

    void PreviewAffectMixtureColor(AffectMixtureColorSO affectColorConsequence, bool showPreview)
    {
        var colorToAffect = affectColorConsequence.colorToAffect;
        var affectValue = affectColorConsequence.affectValue;

        if (!TryGetPreview(colorToAffect, out var colorPreview))
        {
            return;
        }

        if (showPreview)
        {
            Debug.Log($"Previewing Affect Color {colorToAffect} - value: {affectValue}");

            colorPreview.PreviewConsequence(affectColorConsequence, colorManager);
        }
        else
        {
            colorPreview.HidePreview();
            Debug.Log($"NOT Previewing Affect Color {colorToAffect} - value: {affectValue}");
        }

        mixtureBar.RebuildLayout();
    }

    void PreviewNullifyMixtureColor(ChangeColorExistenceSO changeColorConsequence, bool showPreview)
    {
        if (!TryGetPreview(changeColorConsequence.colorItemID, out var colorPreview))
        {
            return;
        }

        if (showPreview)
        {
            Debug.Log($"Previewing Nullify Color {changeColorConsequence}");

            colorPreview.PreviewConsequence(changeColorConsequence, colorManager);
        }
        else
        {
            colorPreview.HidePreview();
            Debug.Log($"NOT Previewing Nullify Color {changeColorConsequence.colorItemID}");
        }

        mixtureBar.RebuildLayout();
    }

    bool TryGetPreview(ColorItemID colorID, out MixtureColorConsequencePreview colorPreview)
    {
        colorPreview = null;
        foreach (MixtureColorConsequencePreview preview in previewsArray)
        {
            if (preview.ColorID == colorID)
            {
                colorPreview = preview;
                return true;
            }
        }

        return false;
    }
}