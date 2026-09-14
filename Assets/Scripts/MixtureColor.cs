using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MixtureColor : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Image colorImg;

    [Header("Animation")] [SerializeField] protected float sizeChangeDuration;

    [SerializeField] protected Ease sizeChangeEase;
    protected float barWidth;
    protected float currentSize;
    protected float maxCapacity;
    MixtureColorSO mixtureColorSO;

    public void SetData(MixtureColorSO mixtureColorSO, float maxCapacity, float barWidth)
    {
        this.mixtureColorSO = mixtureColorSO;
        currentSize = mixtureColorSO.size;
        colorImg.color = mixtureColorSO.colorItemSO.colorValue;

        this.maxCapacity = maxCapacity;
        this.barWidth = barWidth;


        SetRectWidth(currentSize / maxCapacity * barWidth);
    }

    public void AddToCurrentSize(float affectValue)
    {
        currentSize += affectValue;
        SetRectWidth(currentSize / maxCapacity * barWidth);

        Debug.Log($"Size Affected: {mixtureColorSO.colorItemSO.colorItemID}, Current Size: {currentSize}");
    }

    public void SetCurrentSize(float v)
    {
        currentSize = v;
        SetRectWidth(currentSize / maxCapacity * barWidth);

        Debug.Log($"Size Set: {mixtureColorSO.colorItemSO.colorItemID}, Current Size: {currentSize}");
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
}