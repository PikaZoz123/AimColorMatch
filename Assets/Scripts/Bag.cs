using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bag : MonoBehaviour
{
    [SerializeField] GameplayColorSO[] cardPool;

    readonly List<GameplayColorSO> cards = new();

    void Awake()
    {
        ResetCards();
    }

    void ResetCards()
    {
        cards.Clear();
        cards.AddRange(cardPool);
    }

    public bool TryDraw(GameplayColorSO requestedCard, out GameplayColorSO card)
    {
        var index = cards.IndexOf(requestedCard);

        if (index >= 0)
        {
            card = cards[index];
            cards.RemoveAt(index);

            if (cards.Count == 0)
            {
                ResetCards();
            }

            return true;
        }

        card = null;
        return false;
    }

    public bool TryDrawRandom(out GameplayColorSO card)
    {
        if (cards.Count == 0)
        {
            ResetCards();
        }

        if (cards.Count == 0)
        {
            card = null;
            return false;
        }

        var index = Random.Range(0, cards.Count);

        card = cards[index];
        cards.RemoveAt(index);

        if (cards.Count == 0)
        {
            ResetCards();
        }

        return true;
    }

    public IEnumerable<GameplayColorSO> GetAvailableCards()
    {
        return cards;
    }
}