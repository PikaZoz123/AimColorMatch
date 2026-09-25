using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour // divides game colors to mixture and gameplay.
{
    [SerializeField] ColorTableSO colorsTableSO;
    [SerializeField] Hand hand;
    [SerializeField] [Range(3, 10)] int minIDCount = 3;
    [SerializeField] List<ColorItemID> mixtureColorIds;

    readonly List<GameplayColorSO> colorsInHand = new();

    void Awake()
    {
        if (mixtureColorIds.Count < minIDCount)
        {
            Debug.LogError($"Can't have a ID count less than {minIDCount}, Check your mixture colors IDs list");
            Debug.Break();
        }
    }

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