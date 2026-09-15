using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    [SerializeField] GameplayColorSO[] cardPool;
    readonly Dictionary<ColorItemID, Stack<GameplayColorSO>> cardsDict = new();

    void Awake()
    {
        ResetCards();
    }

    void ResetCards()
    {
        cardsDict.Clear();
        foreach (var c in cardPool)
        {
            if (!cardsDict.TryAdd(c.colorItemSO.colorItemID, null))
            {
                cardsDict[c.colorItemSO.colorItemID].Push(c);
            }
            else
            {
                var stack = new Stack<GameplayColorSO>();
                stack.Push(c);
                cardsDict[c.colorItemSO.colorItemID] = stack;
            }
        }
    }


    public bool TryDraw(ColorItemID cardID, out GameplayColorSO card)
    {
        if (cardsDict.TryGetValue(cardID, out var cardStack))
        {
            if (cardStack.TryPop(out card))
            {
                if (cardStack.Count == 0)
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

        card = null;
        return false;
    }

    public bool TryDrawRandom(out GameplayColorSO card)
    {
        var cardID = GetRandomIDFromCardsDict();
        return TryDraw(cardID, out card);
    }

    ColorItemID GetRandomIDFromCardsDict()
    {
        var keys = cardsDict.Keys;
        return keys.ElementAt(Random.Range(0, keys.Count));
    }

    public IEnumerable<GameplayColorSO> GetAvailableCards()
    {
        foreach (var stack in cardsDict.Values)
        {
            foreach (var card in stack)
            {
                yield return card;
            }
        }
    }
}