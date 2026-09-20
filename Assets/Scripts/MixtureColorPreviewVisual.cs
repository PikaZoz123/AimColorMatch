using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MixtureColorPreviewVisual : MixtureColor
{
    [SerializeField] MMPositionShaker shakeFeedback;
    float previewSize;
    Tweener sizeTween;


    public void Animate()
    {
        shakeFeedback.Play();

        var newSizeDelta = rectTransform.sizeDelta;
        newSizeDelta.y = previewSize / maxCapacity * barWidth;

        var oldSizeDelta = rectTransform.sizeDelta;
        oldSizeDelta.y = currentSize / maxCapacity * barWidth;

        sizeTween = rectTransform.DOSizeDelta(newSizeDelta, sizeChangeDuration).From(oldSizeDelta).SetEase(sizeChangeEase);

        valText.text = $"{currentSize}\n|\n{previewSize}";
    }

    public void SetData(float currentSize, float maxCapacity, float barWidth, Color color)
    {
        colorImg.color = color;

        this.maxCapacity = maxCapacity;
        this.barWidth = barWidth;

        SetCurrentSize(currentSize);
    }

    public void SetPreviewSize(float previewSize)
    {
        this.previewSize = Mathf.Max(previewSize, 0);
    }

    public void Deactivate()
    {
        shakeFeedback.Stop();
        sizeTween.Kill();
    }
}