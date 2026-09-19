using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    [SerializeField] ColorItemID[] cardPoolIDList;
    readonly Dictionary<ColorItemID, int> cardsDict = new();

    void Awake()
    {
        ResetCards();
    }

    void ResetCards()
    {
        cardsDict.Clear();
        foreach (var colorID in cardPoolIDList)
        {
            if (!cardsDict.TryAdd(colorID, 1))
            {
                cardsDict[colorID]++;
            }
        }
    }


    public bool TryDraw(ColorItemID cardID)
    {
        if (cardsDict.TryGetValue(cardID, out var cardCount))
        {
            if (cardCount > 0)
            {
                cardsDict[cardID]--;
                if (cardsDict[cardID] == 0)
                {
                    cardsDict.Remove(cardID);
                    if (cardsDict.Count == 0)
                    {
                        ResetCards();
                    }
                }

                return true;
            }
        }

        return false;
    }

    public bool TryDrawRandom(out ColorItemID cardID)
    {
        cardID = GetRandomIDFromCardsDict();
        return TryDraw(cardID);
    }

    ColorItemID GetRandomIDFromCardsDict()
    {
        var keys = cardsDict.Keys;
        return keys.ElementAt(Random.Range(0, keys.Count));
    }

    public IEnumerable<ColorItemID> GetAvailableCards()
    {
        foreach (var (id, count) in cardsDict)
        {
            if (count == 0)
            {
                continue;
            }

            yield return id;
        }
    }
}