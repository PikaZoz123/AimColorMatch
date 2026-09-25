using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hand : MonoBehaviour
{
    [SerializeField] Bag bag;
    [SerializeField] MixtureBar mixtureBar;

    [SerializeField] Evaluator evaluator;
    [SerializeField] HandSettingsSO handSettingsSo;

    readonly List<GameplayColorSO> cards = new();

    public List<GameplayColorSO> Create()
    {
        cards.Clear();

        var handSize = handSettingsSo.handSize;

        while (cards.Count < handSize)
        {
            PullNewCard(
                cards.Count < handSettingsSo.initialStateDrivenCount
            );
        }

        return cards;
    }

    public GameplayColorSO Replace(GameplayColorSO consumedCard)
    {
        cards.Remove(consumedCard);

        return PullNewCard(Random.Range(0f, 1f) <= handSettingsSo.replacementStateDrivenChance);
    }

    GameplayColorSO PullNewCard(bool evaluatorCondition)
    {
        GameplayColorSO selectedCard = null;

        if (evaluatorCondition)
        {
            selectedCard = evaluator.SelectBest(
                bag.GetAvailableCards(),
                mixtureBar.GetMixtureColorsState(),
                mixtureBar.GetCapacity(),
                cards
            );

            if (selectedCard != null &&
                bag.TryDraw(selectedCard, out var card))
            {
                cards.Add(card);
                return card;
            }
        }
        else
        {
            if (bag.TryDrawRandom(out var card))
            {
                cards.Add(card);
                return card;
            }
        }

        return null;
    }

    public List<GameplayColorSO> GetCards()
    {
        return new List<GameplayColorSO>(cards);
    }
}