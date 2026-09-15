using System.Collections.Generic;
using UnityEngine;

public class Evaluator : MonoBehaviour
{
    [SerializeField] EvaluatorBalanceProfileSO balanceProfileSo;


    public ColorItemID SelectBest(IEnumerable<GameplayColorSO> availableBagCards, Dictionary<ColorItemID, float> mixtureBarState, float mixtureBarCapacity, List<GameplayColorSO> handCards)
    {
        ColorItemID bestCardID = default;
        var highestScore = float.MinValue;
        foreach (var card in availableBagCards)
        {
            if (handCards.Exists(
                    x => x.colorItemSO.colorItemID == card.colorItemSO.colorItemID))
            {
                continue;
            }

            var score = Evaluate(card, mixtureBarCapacity, mixtureBarState);
            if (score > highestScore)
            {
                highestScore = score;
                bestCardID = card.colorItemSO.colorItemID;
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

        score += Mathf.RoundToInt(
            totalPressure * balanceProfileSo.totalPressureWeight
        );

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
            }
        }

        return simulatedState;
    }
}