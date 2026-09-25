using System.Collections.Generic;
using UnityEngine;

public class FlushHandler : MonoBehaviour
{
    [SerializeField] MixtureBar mixtureBar;
    [SerializeField] Reconfigurator reconfigurator;


    [SerializeField] [Range(3, 10)] int minAdjacentTowers = 3;
    List<int> flushableTowersList;
    List<int> survivingTowersList;

    public void E_MixtureBar_OnColorsStateChanged()
    {
        var colorsState = mixtureBar.GetMixtureColorsState();
        if (TryFlushableTowers(colorsState, minAdjacentTowers, out flushableTowersList, out survivingTowersList))
        {
            TriggerFlush();


            var newSizes = reconfigurator.Reconfig(colorsState, flushableTowersList, mixtureBar.GetCapacity());

            mixtureBar.SetColorsSize(flushableTowersList, newSizes);

            flushableTowersList.Clear();
        }
    }

    public static bool TryFlushableTowers(Dictionary<ColorItemID, float> state,
        int minAdjacentTowers, out List<int> flushableTowersList, out List<int> survivingTowersList)
    {
        flushableTowersList = new List<int>();
        survivingTowersList = new List<int>();
        var currentTargetSize = -1f;
        var triggerFlush = false;
        var i = 0;

        foreach (var (id, size) in state)
        {
            survivingTowersList.Add(i);
            if (Mathf.Approximately(currentTargetSize, size))
            {
                flushableTowersList.Add(i);
                survivingTowersList.Remove(i);
            }
            else
            {
                if (flushableTowersList.Count >= minAdjacentTowers)
                {
                    triggerFlush = true;
                    break;
                }

                currentTargetSize = size;
                flushableTowersList.Clear();

                flushableTowersList.Add(i);
                survivingTowersList.Remove(i);
            }

            i++;
        }

        if (!triggerFlush && flushableTowersList.Count >= minAdjacentTowers)
        {
            triggerFlush = true;
        }

        return triggerFlush;
    }

    void TriggerFlush()
    {
        Debug.Log("Trigger Flu7sh now !!!");
        mixtureBar.NullifyColors(flushableTowersList);
    }
}