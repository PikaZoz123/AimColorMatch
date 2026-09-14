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
        newSizeDelta.x = previewSize / maxCapacity * barWidth;

        var oldSizeDelta = rectTransform.sizeDelta;
        oldSizeDelta.x = currentSize / maxCapacity * barWidth;

        sizeTween = rectTransform.DOSizeDelta(newSizeDelta, sizeChangeDuration).From(oldSizeDelta).SetLoops(-1, LoopType.Restart).SetEase(sizeChangeEase);
    }

    public void SetPreviewSize(float previewSize)
    {
        this.previewSize = previewSize;
    }

    public void Deactivate()
    {
        shakeFeedback.Stop();
        sizeTween.Kill();
    }
}