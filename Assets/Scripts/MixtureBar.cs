using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MixtureBar : ConsequenceHandler
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform contentParent;
    [SerializeField] MixtureColor mixtureColorPrefab;
    [SerializeField] float capacity;

    [HideInInspector] public UnityEvent<Dictionary<ColorItemID, MixtureColor>> onColorsInitialized;
    readonly List<ColorItemID> colorsOrderList = new();
    readonly Dictionary<ColorItemID, MixtureColor> mixtureColorsDict = new();


    protected void Start()
    {
        InitializeColors(colorManager.GetMixtureColorsData());
    }

    void OnDestroy()
    {
        onColorsInitialized?.RemoveAllListeners();
    }

    void InitializeColors(List<MixtureColorSO> mixtureColorsData)
    {
        foreach (var data in mixtureColorsData)
        {
            var copy = Instantiate(mixtureColorPrefab, contentParent);
            copy.SetData(data, capacity, rectTransform.rect.width);

            var id = data.colorItemSO.colorItemID;
            mixtureColorsDict.Add(id, copy);

            colorsOrderList.Add(id);
        }

        onColorsInitialized?.Invoke(mixtureColorsDict);
    }

    protected override void OnConsequencesHappened(ConsequenceItemSO[] consequencesArray)
    {
        foreach (var consequence in consequencesArray)
        {
            if (consequence is ChangeColorExistenceSO { colorExistenceID: ColorExistenceID.RemoveMixtureColor } changeColorExistence) // checks if generate new color existence consequence
            {
                NullifyMixtureColor(changeColorExistence.colorItemID);
            }
            else if (consequence is AffectMixtureColorSO affectColor)
            {
                AffectMixtureColor(affectColor.colorToAffect, affectColor.affectValue);
            }
        }
    }


    void AffectMixtureColor(ColorItemID colorToAffect, float affectValue)
    {
        if (TryGetColor(colorToAffect, out var color))
        {
            color.AddToCurrentSize(affectValue);
            RebuildLayout();
        }
    }

    void NullifyMixtureColor(ColorItemID colorItemID)
    {
        if (TryGetColor(colorItemID, out var color))
        {
            color.SetCurrentSize(0);
            RebuildLayout();
        }
    }

    public void RebuildLayout()
    {
        LayoutRebuilder.MarkLayoutForRebuild(contentParent);
    }


    bool TryGetColor(ColorItemID colorID, out MixtureColor color)
    {
        return mixtureColorsDict.TryGetValue(colorID, out color);
    }

    public Dictionary<ColorItemID, float> GetMixtureColorsState()
    {
        var dataDict = new Dictionary<ColorItemID, float>();
        foreach (var (id, color) in mixtureColorsDict)
        {
            dataDict.Add(id, color.GetCurrentSize());
        }

        return dataDict;
    }

    public float GetCapacity()
    {
        return capacity;
    }

    public List<ColorItemID> GetTowerOrder()
    {
        return colorsOrderList;
    }
}