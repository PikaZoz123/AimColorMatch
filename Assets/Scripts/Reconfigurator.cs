using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reconfigurator : MonoBehaviour
{
    [SerializeField] ReconfiguratorBalanceProfileSO balanceProfileSo;
    [SerializeField] Hand hand;
    List<GameplayColorSO> handCards;


    public Dictionary<int, List<float>> Reconfig(Dictionary<ColorItemID, float> colorsState, List<int> flushedTowersList, float maxCapacity)
    {
        Debug.Log("Reconfiguring with new values - TBD");

        var moveCount = DecideMoveCount();
        Debug.Log($"Move count decided: {moveCount}");

        var sizes = new Dictionary<int, List<float>>();
        if (moveCount > 0)
        {
            handCards = hand.GetCards();

            var usableToolsComibnations = GetUsableToolsCombo(moveCount);
            Debug.Log($"Got Tools Combinations: {usableToolsComibnations.Count}");

            var processedWindowsList = GetComboProcessedWindowsList(colorsState, usableToolsComibnations);

            sizes = GetFlushTowerRefillValues(flushedTowersList, maxCapacity, processedWindowsList);
        }
        else
        {
            sizes = RandomRefill(flushedTowersList, maxCapacity);
        }

        return sizes;
    }

    Dictionary<int, List<float>> RandomRefill(List<int> flushedTowersList, float maxCapacity)
    {
        var sizes = new Dictionary<int, List<float>>();

        foreach (var flushedTower in flushedTowersList)
        {
            var randomSize = Random.Range(maxCapacity * balanceProfileSo.randomRefillMinBias, maxCapacity * balanceProfileSo.randomRefillMaxBias);
            randomSize = Mathf.RoundToInt(randomSize);
            sizes.TryAdd(flushedTower, new List<float>
            {
                randomSize
            });
            Debug.Log($"New Size for tower index: {flushedTower} - Random {randomSize}");
        }

        return sizes;
    }


    List<List<FlushCandidate>> GetComboProcessedWindowsList(Dictionary<ColorItemID, float> colorsState,
        List<List<GameplayColorSO>> usableToolsComibnations)
    {
        var list = new List<List<FlushCandidate>>();

        var i = 0;
        foreach (var combo in usableToolsComibnations) // for every combo,
            // generate windows (every window is a range of towers eg: 0-2), add em to a list.
        {
            var simulatedState = SimulateComboEffects(combo, colorsState);

            if (FindFlushCandidates(out var processedWindows,
                    simulatedState.Values.ToList(), 3, combo, i))
            {
                list.Add(processedWindows);
            }

            i++;
        }

        return list;
    }

    Dictionary<int, List<float>> GetFlushTowerRefillValues(List<int> flushedTowersList,
        float maxCapacity, List<List<FlushCandidate>> comboCandidatesList)
    {
        var sizes = new Dictionary<int, List<float>>();

        foreach (var flushedTower in flushedTowersList)
        {
            sizes.TryAdd(flushedTower, null);

            foreach (var windows in comboCandidatesList)
            {
                foreach (var window in windows)
                {
                    if (window.mismatchedTowerIndex == flushedTower)
                    {
                        var l = sizes[flushedTower];
                        if (l == null)
                        {
                            sizes[flushedTower] = new List<float> { window.missingAmount };
                        }
                        else
                        {
                            sizes[flushedTower].Add(window.missingAmount);
                        }

                        Debug.Log($"New Size for tower index: {flushedTower} - {window.missingAmount} - Window Range {window.startIndex} - {window.endIndex}");
                    }
                }
            }

            if (sizes[flushedTower] == null) // check if flushed tower is not found in the windows, just assign a random value
            {
                var randSize = Random.Range(1, (int)maxCapacity);
                sizes[flushedTower] = new List<float> { randSize };
                Debug.Log($"Not found in window, New Random Size for tower index: {flushedTower} - {randSize}");
            }
        }

        return sizes;
    }

    bool FindFlushCandidates(out List<FlushCandidate> processedWindows, List<float> simulatedStateValues, int minAdjacentTowers,
        List<GameplayColorSO> combo, int comboIndex)
    {
        processedWindows = null;

        var windowLimitIndex = simulatedStateValues.Count - minAdjacentTowers;

        var indexedValuesArray = new int[simulatedStateValues.Count];
        for (var i = 0; i < indexedValuesArray.Length; i++)
        {
            indexedValuesArray[i] = i;
        }

        ChooseWindows(ref processedWindows);

        return processedWindows != null;

        void ChooseWindows(ref List<FlushCandidate> processedWindows)
        {
            for (var i = 0; i <= windowLimitIndex; i++)
            {
                var window = new FlushCandidate(i, i + minAdjacentTowers - 1);

                if (ProcessTowersInWindow(ref window))
                {
                    processedWindows ??= new List<FlushCandidate>();

                    processedWindows.Add(window);
                    Debug.Log($"Candidates found in window range: {window.startIndex} - {window.endIndex}" +
                              $" mismatched tower index: {window.mismatchedTowerIndex} - missing amount: {window.missingAmount}" +
                              $" Combo: {GetComboTools()}");
                }
                else
                {
                    Debug.Log($"No candidates in window range: {window.startIndex} - {window.endIndex}" +
                              $" Combo: {GetComboTools()}");
                }
            }
        }

        string GetComboTools()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append($"Index {comboIndex} - Tools: ");
            for (var i = 0; i < combo.Count; i++)
            {
                var t = combo[i];
                strBuilder.Append($"{t.name}");
                if (i < combo.Count - 1)
                {
                    strBuilder.Append(" - ");
                }
            }

            return strBuilder.ToString();
        }


        bool ProcessTowersInWindow(ref FlushCandidate window)
        {
            var identicalTowersInWindow = new List<(int, int)>();

            var windowStart = indexedValuesArray[window.startIndex];
            var windowEnd = indexedValuesArray[window.endIndex];

            for (var a = windowStart; a <= windowEnd; a++)
            {
                for (var b = windowStart; b <= windowEnd; b++)
                {
                    if (a == b)
                    {
                        continue;
                    }

                    var sizeA = simulatedStateValues[a];
                    var sizeB = simulatedStateValues[b];

                    if (Mathf.Approximately(sizeA, sizeB) && !DoIdenticalTowersContainPair(a, b))
                    {
                        identicalTowersInWindow.Add(new ValueTuple<int, int>(a, b)); // an array can be used,
                        // but using a tuple since we only have 2 comparable values
                    }
                }
            }

            if (identicalTowersInWindow.Count == 0)
            {
                return false;
            }

            for (var a = windowStart; a <= windowEnd; a++)
            {
                if (!DoIdenticalTowersContainIndex(a))
                {
                    window.mismatchedTowerIndex = a;

                    var targetSize = simulatedStateValues[identicalTowersInWindow[0].Item1];
                    var missingTowerSize = simulatedStateValues[a];

                    window.missingAmount = Mathf.Abs(targetSize - missingTowerSize);
                    window.targetValue = targetSize;

                    return true;
                }
            }

            return false;

            bool DoIdenticalTowersContainIndex(int index)
            {
                foreach (var (item1, item2) in identicalTowersInWindow)
                {
                    if (item1 == index || item2 == index)
                    {
                        return true;
                    }
                }

                return false;
            }

            bool DoIdenticalTowersContainPair(int a, int b)
            {
                foreach (var (item1, item2) in identicalTowersInWindow)
                {
                    if ((a == item1 && b == item2) || (a == item2 && b == item1))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }


    Dictionary<ColorItemID, float> SimulateComboEffects(List<GameplayColorSO> combo,
        Dictionary<ColorItemID, float> originalState)
    {
        var result = new Dictionary<ColorItemID, float>(originalState);

        foreach (var card in combo)
        {
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

                result[affect.colorToAffect] = Mathf.Max(0f, result[affect.colorToAffect]);
            }
        }

        return result;
    }

    List<List<GameplayColorSO>> GetUsableToolsCombo(int moveCount)
    {
        if (moveCount <= 0)
        {
            return null; // for now
        }

        var comboList = new List<List<GameplayColorSO>>();

        var combo = new List<GameplayColorSO>();

        FillComboList(moveCount, 0, ref combo, comboList);


        return comboList;
    }

    void FillComboList(int moveCount, int index, ref List<GameplayColorSO> combo, List<List<GameplayColorSO>> comboList)
    {
        bool DuplicateCombo(List<GameplayColorSO> combo)
        {
            var duplicate = true;
            foreach (var c in comboList)
            {
                foreach (var t in c)
                {
                    if (!combo.Contains(t))
                    {
                        duplicate = false;
                        break;
                    }
                }
            }

            return comboList.Count > 0 && duplicate;
        }

        if (combo.Count >= moveCount)
        {
            if (!DuplicateCombo(combo))
            {
                comboList.Add(new List<GameplayColorSO>(combo));
            }

            return;
        }

        if (index >= handCards.Count)
        {
            return;
        }

        for (var i = index; i < handCards.Count; i++)
        {
            var tool = handCards[i];

            if (combo.Contains(tool))
            {
                continue;
            }

            combo.Add(tool);

            FillComboList(moveCount, i + 1, ref combo, comboList);


            combo.RemoveAt(combo.Count - 1);
        }
    }


    int DecideMoveCount()
    {
        var rand = Random.Range(0, 100f);
        if (rand < balanceProfileSo.oneMoveOpportunityChance)
        {
            return 1;
        }

        if (rand < balanceProfileSo.oneMoveOpportunityChance + balanceProfileSo.twoMoveOpportunityChance)
        {
            return 2;
        }

        if (rand < balanceProfileSo.oneMoveOpportunityChance +
            balanceProfileSo.twoMoveOpportunityChance +
            balanceProfileSo.threeMoveOpportunityChance)
        {
            return 3;
        }

        return 0;
    }


    struct FlushCandidate
    {
        public readonly int startIndex;
        public readonly int endIndex;

        public float targetValue;

        public float missingAmount;

        public int mismatchedTowerIndex;

        public FlushCandidate(int startIndex, int endIndex)
        {
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            mismatchedTowerIndex = -1;
            targetValue = -1;
            missingAmount = 0;
        }
    }
}