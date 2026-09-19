using System.Collections.Generic;
using UnityEngine;

public class Evaluator : MonoBehaviour
{
    [SerializeField] ColorManager colorManager;
    [SerializeField] EvaluatorBalanceProfileSO balanceProfileSo;


    public ColorItemID SelectBest(IEnumerable<ColorItemID> availableBagCards, Dictionary<ColorItemID, float> mixtureBarState,
        float mixtureBarCapacity, List<ColorItemID> handCards)
    {
        ColorItemID bestCardID = default;
        var highestScore = float.MinValue;
        foreach (var cardID in availableBagCards)
        {
            if (handCards.Contains(cardID))
            {
                continue;
            }

            var score = Evaluate(colorManager.GetOneGameplayColorData(cardID), mixtureBarCapacity, mixtureBarState);
            if (score > highestScore)
            {
                highestScore = score;
                bestCardID = cardID;
            }
        }

        return bestCardID;
    }

    float Evaluate(GameplayColorSO card, float mixtureBarCapacity, Dictionary<ColorItemID, float> mixtureBarState)
    {
        var result = Simulate(card, mixtureBarState);
        var score = 0f;
        var totalPressure = 0f;
        var flushCount = 0;

        foreach (var (id, before) in mixtureBarState)
        {
            var after = result[id];

            totalPressure += after;

            if (before > 0 && after <= 0)
            {
                flushCount++;
            }
        }

        score += flushCount * balanceProfileSo.flushValue;

        score += totalPressure * balanceProfileSo.totalPressureWeight;

        if (totalPressure >= mixtureBarCapacity)
        {
            score += balanceProfileSo.deathPenalty;
        }

        return score;
    }

    Dictionary<ColorItemID, float> Simulate(GameplayColorSO card, Dictionary<ColorItemID, float> mixtureBarState)
    {
        var simulatedState = new Dictionary<ColorItemID, float>(mixtureBarState);

        foreach (var consequence in card.consequencesArray)
        {
            if (consequence is AffectMixtureColorSO affect &&
                simulatedState.ContainsKey(affect.colorToAffect))
            {
                simulatedState[affect.colorToAffect] += affect.affectValue;
                simulatedState[affect.colorToAffect] = Mathf.Max(0, simulatedState[affect.colorToAffect]);
            }
        }

        return simulatedState;
    }
}