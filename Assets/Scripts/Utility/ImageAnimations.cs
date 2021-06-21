using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageAnimations : Singleton<ImageAnimations>
{
    public float EaseInBounceDuration;
    public float BounceDuration;

    [SerializeField] private AnimationCurve bounceAnimationCurve;
    [SerializeField] private AnimationCurve easeInBounceAnimationCurve;

    public Action<Image> GetAnimationWithCurve<TProperty>(Func<Image, TProperty> getter, Action<Image, TProperty, float> setter, AnimationCurve curve, float duration)
    {
        Action<Image> animation = (image) =>
        {
            var initialPropertyValue = getter(image);
            setter(image, initialPropertyValue, 0);
            float animationCurvePosition = 0.0f;
            DOTween.To(
                () => animationCurvePosition,
                value =>
                {
                    setter(image, initialPropertyValue, curve.Evaluate(animationCurvePosition));
                    animationCurvePosition = value;
                },
                1.0f,
                duration);
        };
        return animation;
    }

    public Action<Image> GetBounce()
    {
        return GetAnimationWithCurve(
            (image) => image.rectTransform.sizeDelta,
            (image, initialValue, curveValue) => image.rectTransform.sizeDelta = initialValue * curveValue,
            bounceAnimationCurve, BounceDuration);
    }

    public Action<Image> GetEaseInBounce()
    {
        return GetAnimationWithCurve(
            (image) => image.rectTransform.sizeDelta,
            (image, initialValue, curveValue) => image.rectTransform.sizeDelta = initialValue * curveValue,
            easeInBounceAnimationCurve, EaseInBounceDuration);
    }
}
