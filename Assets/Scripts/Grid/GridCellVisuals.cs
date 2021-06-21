using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class GridCellVisuals
{
    [SerializeField] private Image rim;
    [SerializeField] private Image fill;
    [SerializeField] private Image item;

    public void SetVisuals(Item item)
    {
        this.item.sprite = item.Sprite;
        fill.color = item.FillColor;
    }

    public void Animate(Action<Image> animation)
    {
        animation(rim);
        animation(fill);
        animation(item);
    }

    public void AnimateItem(Action<Image> animation)
    {
        animation(item);
    }
}