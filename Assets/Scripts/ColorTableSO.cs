using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ColorTableSO : ScriptableObject
{
    [SerializeField] GameplayColorSO[] gameplayColors; // should hold aaaall the gameplay colors so 
    [SerializeField] MixtureColorSO[] mixtureColors; // should hold aaaall the mixture colors so 
    [SerializeField] ColorItemSO[] allColors; // should hold aaaall the color items so 

    public GameplayColorSO[] GetGameplayColors(List<ColorItemID> idsList)
    {
        var t = new GameplayColorSO[idsList.Count];

        for (var i = 0; i < idsList.Count; i++)
        {
            var id = idsList[i];
            t[i] = gameplayColors.First(x => x.colorItemSO.colorItemID == id);
        }

        return t;
    }

    public MixtureColorSO[] GetMixtureColors(List<ColorItemID> idsList)
    {
        var t = new MixtureColorSO[idsList.Count];

        for (var i = 0; i < idsList.Count; i++)
        {
            var id = idsList[i];
            t[i] = mixtureColors.First(x => x.colorItemSO.colorItemID == id);
        }

        return t;
    }

    public ColorItemSO[] GetColors(List<ColorItemID> idsList)
    {
        var t = new ColorItemSO[idsList.Count];

        for (var i = 0; i < idsList.Count; i++)
        {
            var id = idsList[i];
            t[i] = allColors.First(x => x.colorItemID == id);
        }

        return t;
    }

    public GameplayColorSO GetGameplayColor(ColorItemID id)
    {
        return gameplayColors.First(x => x.colorItemSO.colorItemID == id);
    }

    public MixtureColorSO GetMixtureColor(ColorItemID id)
    {
        return mixtureColors.First(x => x.colorItemSO.colorItemID == id);
    }
}