using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Evaluator : MonoBehaviour
{
    [SerializeField] EvaluatorBalanceProfileSO balanceProfileSo;

    public GameplayColorSO SelectBest(IEnumerable<GameplayColorSO> availableBagCards,
        Dictionary<ColorItemID, float> towerState,
        float towerCapacity,
        List<GameplayColorSO> handCards)
    {
        GameplayColorSO bestCard = null;
        var highestScore = float.MinValue;

        foreach (var card in availableBagCards)
        {
            // Do not offer a tool that is already in the hand.
            if (handCards.Contains(card))
            {
                continue;
            }

            var score = Evaluate(
                card,
                towerState,
                towerCapacity
            );

            if (score > highestScore)
            {
                highestScore = score;
                bestCard = card;
            }
        }

        return bestCard;
    }

    float Evaluate(GameplayColorSO card, Dictionary<ColorItemID, float> towerState, float towerCapacity)
    {
        var simulatedState = Simulate(card, towerState);

        var beforeValues = towerState.Values.ToList();
        var simulatedValues = simulatedState.Values.ToList();

        var beforeOpportunity = EvaluateOpportunity(beforeValues);
        var afterOpportunity = EvaluateOpportunity(simulatedValues);

        var opportunityImprovement = afterOpportunity - beforeOpportunity;

        var beforePressure = EvaluatePressure(towerState, towerCapacity);

        var afterPressure = EvaluatePressure(simulatedState, towerCapacity);

        var pressureChange = afterPressure - beforePressure;

        var beforeFlushes = CountFlushes(beforeValues);
        var afterFlushes = CountFlushes(simulatedValues);

        var newFlushes = afterFlushes - beforeFlushes;

        var score = 0f;

        // Main purpose:
        // create or improve visible opportunities.
        score += opportunityImprovement;

        // Completing a flush is especially valuable.
        score += newFlushes * balanceProfileSo.flushValue;

        // Avoid pushing towers toward dangerous capacity.
        score -= pressureChange * balanceProfileSo.totalPressureWeight;

        return score;
    }


    Dictionary<ColorItemID, float> Simulate(
        GameplayColorSO card,
        Dictionary<ColorItemID, float> towerState)
    {
        var result = new Dictionary<ColorItemID, float>(towerState);

        foreach (var consequence in card.consequencesArray)
        {
            if (consequence is not AffectMixtureColorSO affect)
            {
                continue;
            }

            if (!result.ContainsKey(affect.colorToAffect))
            {
                continue;
            }

            result[affect.colorToAffect] += affect.affectValue;

            result[affect.colorToAffect] =
                Mathf.Max(0f, result[affect.colorToAffect]);
        }

        return result;
    }

    float EvaluateOpportunity(List<float> values)
    {
        var score = 0f;

        for (var i = 0; i <= values.Count - 3; i++)
        {
            var a = values[i];
            var b = values[i + 1];
            var c = values[i + 2];

            var spread =
                Mathf.Max(a, b, c) -
                Mathf.Min(a, b, c);

            // Already aligned.
            if (Mathf.Approximately(spread, 0f))
            {
                score += balanceProfileSo.flushValue;
            }
            // One point away from alignment.
            else if (Mathf.Approximately(spread, 1f))
            {
                score += balanceProfileSo.nearAlignmentValue;
            }
        }

        return score;
    }

    int CountFlushes(List<float> values)
    {
        var count = 0;

        for (var i = 0; i <= values.Count - 3; i++)
        {
            if (Mathf.Approximately(values[i], values[i + 1]) &&
                Mathf.Approximately(values[i + 1], values[i + 2]))
            {
                count++;
            }
        }

        return count;
    }

    float EvaluatePressure(
        Dictionary<ColorItemID, float> towerState,
        float capacity)
    {
        var pressure = 0f;

        foreach (var value in towerState.Values)
        {
            // Normal pressure.
            pressure += value;

            // Reaching capacity is catastrophic.
            if (value >= capacity)
            {
                pressure += balanceProfileSo.deathPenalty;
            }
        }

        return pressure;
    }
}