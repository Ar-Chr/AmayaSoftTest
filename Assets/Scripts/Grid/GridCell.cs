using UnityEngine;

public class GridCell : MonoBehaviour
{
    public string ItemName { get; private set; }

    [SerializeField] private GridCellVisuals visuals;

    private bool chosen;

    private void Start()
    {
        LevelController.Instance.
            answerEvaluatedEvent.AddListener(OnAnswerEvaluated);
    }

    public void Initialize(Item item)
    {
        ItemName = item.Name;
        visuals.SetVisuals(item);
    }

    public void Bounce() => visuals.Animate(ImageAnimations.Instance.GetBounce());

    public void OnCellChosen()
    {
        chosen = true;
        LevelController.Instance.answerPickedEvent.Invoke(ItemName);
    }

    private void OnAnswerEvaluated(bool correct)
    {
        if (!chosen)
            return;

        if (correct)
        {
            visuals.AnimateItem(ImageAnimations.Instance.GetBounce());
        }
        else
        {
            visuals.AnimateItem(ImageAnimations.Instance.GetEaseInBounce());
        }

        chosen = false;
    }
}
