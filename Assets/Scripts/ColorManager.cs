using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour // divides game colors to mixture and gameplay.
{
    [SerializeField] ColorTableSO colorsTableSO;
    [SerializeField] int totalColorCount = 6;
    [SerializeField] int mixtureColorsCount = 3;
    [SerializeField] int gameplayColorsCount = 3;
    [SerializeField] List<ColorItemID> mixtureColorsList = new();
    [SerializeField] List<ColorItemID> gameplayColorsList = new();
    [SerializeField] bool initializeRandomColors;


    void Awake()
    {
        if (mixtureColorsCount == 0 || gameplayColorsCount == 0)
        {
            Debug.LogError("Mix or Game Colors count can't be 0");
            return;
        }

        if (mixtureColorsCount + gameplayColorsCount > totalColorCount)
        {
            Debug.LogError("Mix + Game Colors count can't be greater than total color count");
            return;
        }

        if (initializeRandomColors)
        {
            InitializeColorsRandomly();
        }
    }

    void InitializeColorsRandomly()
    {
        mixtureColorsList.Clear();
        gameplayColorsList.Clear();

        var r = 0;
        var previousR = -1;

        for (var i = 0; i < mixtureColorsCount; i++)
        {
            do
            {
                r = Random.Range(0, totalColorCount);
            } while (r == previousR);

            previousR = r;

            mixtureColorsList.Add((ColorItemID)r);
        }

        r = 0;
        previousR = -1;

        for (var i = 0; i < gameplayColorsCount; i++)
        {
            do
            {
                r = Random.Range(0, totalColorCount);
            } while (r == previousR || mixtureColorsList.Contains((ColorItemID)r));

            previousR = r;

            gameplayColorsList.Add((ColorItemID)r);
        }
    }

    public GameplayColorSO[] GetGameplayColorsData()
    {
        return colorsTableSO.GetGameplayColors(gameplayColorsList);
    }

    public MixtureColorSO[] GetMixtureColorsData()
    {
        return colorsTableSO.GetMixtureColors(mixtureColorsList);
    }

    public GameplayColorSO GetOneGameplayColorData(ColorItemID colorItemID)
    {
        return colorsTableSO.GetGameplayColor(colorItemID);
    }

    public MixtureColorSO GetOneMixtureColorData(ColorItemID colorItemID)
    {
        return colorsTableSO.GetMixtureColor(colorItemID);
    }
}