using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixtureColor : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Image colorImg;
    [SerializeField] protected TMP_Text valText;

    [Header("Animation")] [SerializeField] protected float sizeChangeDuration;

    [SerializeField] protected Ease sizeChangeEase;
    protected float barWidth;
    protected float currentSize;
    protected float maxCapacity;
    MixtureColorSO mixtureColorSO;

    public void SetData(MixtureColorSO mixtureColorSO, float maxCapacity, float barWidth)
    {
        this.mixtureColorSO = mixtureColorSO;
        colorImg.color = mixtureColorSO.colorItemSO.colorValue;

        this.maxCapacity = maxCapacity;
        this.barWidth = barWidth;

        SetCurrentSize(mixtureColorSO.size);
    }

    public void AddToCurrentSize(float affectValue)
    {
        currentSize += affectValue;
        currentSize = Mathf.Max(0, currentSize);
        valText.text = $"{currentSize}";


        SetRectWidth(currentSize / maxCapacity * barWidth);
    }

    public void SetCurrentSize(float v)
    {
        currentSize = Mathf.Max(0, v);
        valText.text = $"{currentSize}";

        SetRectWidth(currentSize / maxCapacity * barWidth);
    }

    void SetRectWidth(float width)
    {
        var newDelta = rectTransform.sizeDelta;
        newDelta.x = width;

        rectTransform.DOSizeDelta(newDelta, sizeChangeDuration);
    }

    public float GetCurrentSize()
    {
        return currentSize;
    }

    public float GetMaxCapacity()
    {
        return maxCapacity;
    }

    public float GetBarWidth()
    {
        return barWidth;
    }


    public void ResetCurrentSize()
    {
        SetCurrentSize(currentSize);
    }

    public Color GetColor()
    {
        return mixtureColorSO.colorItemSO.colorValue;
    }
}