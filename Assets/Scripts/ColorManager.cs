using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour // divides game colors to mixture and gameplay.
{
    [SerializeField] ColorTableSO colorsTableSO;
    [SerializeField] Hand hand;
    [SerializeField] int totalColorCount = 6;
    [SerializeField] int mixtureColorsCount = 3;
    [SerializeField] int gameplayColorsCount = 3;
    [SerializeField] List<ColorItemID> mixtureColorsList = new();
    [SerializeField] bool initializeRandomColors;
    readonly List<GameplayColorSO> colorsInHand = new();


    public MixtureColorSO[] GetMixtureColorsData()
    {
        return colorsTableSO.GetMixtureColors(mixtureColorsList);
    }

    public GameplayColorSO GetOneGameplayColorData(ColorItemID colorItemID)
    {
        return colorsTableSO.GetGameplayColor(colorItemID);
    }

    public MixtureColorSO GetOneMixtureColorData(ColorItemID colorItemID)
    {
        return colorsTableSO.GetMixtureColor(colorItemID);
    }

    public List<GameplayColorSO> GetColorsInHand()
    {
        colorsInHand.Clear();
        var idsInHand = hand.Create();

        foreach (var id in idsInHand)
        {
            colorsInHand.Add(GetOneGameplayColorData(id));
        }

        return colorsInHand;
    }

    public GameplayColorSO GetConsumedCardReplacement(ColorItemID id)
    {
        return GetOneGameplayColorData(hand.Replace(id));
    }
}