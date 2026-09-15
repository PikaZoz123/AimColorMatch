using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] Bag bag;
    [SerializeField] MixtureBar mixtureBar;

    [SerializeField] Evaluator evaluator;
    [SerializeField] HandSettingsSO handSettingsSo;

    readonly List<GameplayColorSO> cards = new();

    void Create()
    {
        cards.Clear();

        var handSize = handSettingsSo.handSize;
        while (cards.Count < handSize)
        {
            PullNewCard(cards.Count < handSettingsSo.initialStateDrivenCount); //ShouldInitiallySelectBasedOnState
        }
    }

    public void Consume(GameplayColorSO consumedCard)
    {
        cards.Remove(consumedCard);

        PullNewCard(Random.Range(0, 1f) <= handSettingsSo.replacementStateDrivenChance);
    }

    void PullNewCard(bool evaluatorCondition)
    {
        GameplayColorSO card = null;
        if (evaluatorCondition)
        {
            var cardID = evaluator.SelectBest(
                bag.GetAvailableCards(),
                mixtureBar.GetMixtureColorsState(),
                mixtureBar.GetCapacity(),
                cards
            );

            if (bag.TryDraw(cardID, out card))
            {
                cards.Add(card);
            }
        }
        else
        {
            if (bag.TryDrawRandom(out card))
            {
                cards.Add(card);
            }
        }
    }
}