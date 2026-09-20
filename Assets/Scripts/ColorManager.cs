using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour // divides game colors to mixture and gameplay.
{
    [SerializeField] ColorTableSO colorsTableSO;
    [SerializeField] Hand hand;
    [SerializeField] int totalColorCount = 6;
    [SerializeField] int mixtureColorsCount = 3;

    [SerializeField] int gameplayColorsCount = 3;
    [SerializeField] List<ColorItemID> mixtureColorIds;

    [SerializeField] bool initializeRandomColors;
    readonly List<GameplayColorSO> colorsInHand = new();


    public List<MixtureColorSO> GetMixtureColorsData()
    {
        var colorsInBar = new List<MixtureColorSO>();
        foreach (var id in mixtureColorIds)
        {
            colorsInBar.Add(GetOneMixtureColorData(id));
        }

        return colorsInBar;
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
        var handCards = hand.Create();

        foreach (var card in handCards)
        {
            colorsInHand.Add(card);
        }

        return colorsInHand;
    }

    public GameplayColorSO GetConsumedCardReplacement(GameplayColorSO consumedCard)
    {
        return hand.Replace(consumedCard);
    }
}