using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StarParticleShooter : ParticleShooter
{
    [SerializeField] private List<Star> stars;
    [Space]
    [SerializeField] private float duration;

    public override void Shoot()
    {
        foreach (var star in stars)
        {
            star.image.gameObject.SetActive(true);
            StartCoroutine(GameController.DelayAction(() => 
            {
                star.image.gameObject.SetActive(false);
                star.image.rectTransform.position = star.initialPosition;
                }, 
                duration));

            var xAnim = ImageAnimations.Instance.GetAnimationWithCurve(
                (image) => image.rectTransform.position,
                (image, initialPos, curveValue) =>
                {
                    var pos = image.rectTransform.position;
                    pos.x = initialPos.x + curveValue;
                    image.rectTransform.position = pos;
                },
                star.xCurve, 1.5f);

            var yAnim = ImageAnimations.Instance.GetAnimationWithCurve(
                (image) => image.rectTransform.position,
                (image, initialPos, curveValue) =>
                {
                    var pos = image.rectTransform.position;
                    pos.y = initialPos.y + curveValue;
                    image.rectTransform.position = pos;
                },
                star.yCurve, 1.5f);

            xAnim(star.image);
            yAnim(star.image);
        }
    }

    [ContextMenu("Set Initial Positions")]
    public void SetInitialPositions()
    {
        foreach (var star in stars)
        {
            star.SetInitialPosition();
        }
    }

    [System.Serializable]
    class Star
    {
        public Image image;
        public AnimationCurve xCurve;
        public AnimationCurve yCurve;

        public Vector3 initialPosition;

        public void SetInitialPosition()
        {
            initialPosition = image.rectTransform.position;
        }
    }
}
