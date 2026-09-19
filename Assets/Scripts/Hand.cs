using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hand : MonoBehaviour
{
    [SerializeField] Bag bag;
    [SerializeField] MixtureBar mixtureBar;

    [SerializeField] Evaluator evaluator;
    [SerializeField] HandSettingsSO handSettingsSo;

    readonly List<ColorItemID> cards = new();

    public List<ColorItemID> Create()
    {
        cards.Clear();

        var handSize = handSettingsSo.handSize;
        while (cards.Count < handSize)
        {
            PullNewCard(cards.Count < handSettingsSo.initialStateDrivenCount); //ShouldInitiallySelectBasedOnState
        }

        return cards;
    }

    public ColorItemID Replace(ColorItemID consumedCardID)
    {
        cards.Remove(consumedCardID);

        return PullNewCard(Random.Range(0, 1f) <= handSettingsSo.replacementStateDrivenChance);
    }

    ColorItemID PullNewCard(bool evaluatorCondition)
    {
        ColorItemID cardID = default;
        if (evaluatorCondition)
        {
            cardID = evaluator.SelectBest(
                bag.GetAvailableCards(),
                mixtureBar.GetMixtureColorsState(),
                mixtureBar.GetCapacity(),
                cards
            );

            if (bag.TryDraw(cardID))
            {
                cards.Add(cardID);
            }
        }
        else
        {
            if (bag.TryDrawRandom(out cardID))
            {
                cards.Add(cardID);
            }
        }

        return cardID;
    }
}