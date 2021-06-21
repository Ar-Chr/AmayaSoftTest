using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
    [SerializeField] private Text taskText;
    [SerializeField] private Image loadingScreen;
    [SerializeField] private Button restartButton;
    [SerializeField] private Image clickBlocker;

    [HideInInspector] public UnityEvent restartButtonClickedEvent;

    private string initialTaskText;

    private bool isInit;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (isInit)
            return;

        GameController.Instance.gameStartedEvent.AddListener(OnGameStarted);
        GameController.Instance.levelStartedEvent.AddListener(OnLevelStarted);
        GameController.Instance.gameFinishedEvent.AddListener(OnGameFinished);
        LevelController.Instance.answerPickedEvent.AddListener(OnAnswerPicked);
        LevelController.Instance.answerEvaluatedEvent.AddListener(OnAnswerEvaluated);
        restartButton.onClick.AddListener(restartButtonClickedEvent.Invoke);
        restartButtonClickedEvent.AddListener(OnRestartButtonClicked);
        initialTaskText = taskText.text;
        var c = taskText.color;
        c.a = 0;
        taskText.color = c;

        isInit = true;
    }

    private void OnAnswerPicked(string answer)
    {
        clickBlocker.gameObject.SetActive(true);
    }

    private void OnAnswerEvaluated(bool correct)
    {
        if (!correct)
            StartCoroutine(GameController.DelayAction(() => clickBlocker.gameObject.SetActive(false), ImageAnimations.Instance.EaseInBounceDuration));
    }

    private void OnRestartButtonClicked()
    {
        restartButton.image.raycastTarget = false;
        restartButton.gameObject.SetActive(false);
    }

    private void OnGameStarted()
    {
        loadingScreen.DOFade(0, 0.5f);
        taskText.DOFade(1.0f, 1.2f).From(0.0f).SetDelay(1.0f);
    }

    private void OnLevelStarted(string correctAnswer)
    {
        clickBlocker.gameObject.SetActive(false);
        taskText.text = string.Format(initialTaskText, correctAnswer);
    }

    private void OnGameFinished()
    {
        loadingScreen.DOFade(1.0f, 1.0f).SetDelay(1.5f);
        restartButton.gameObject.SetActive(true);
        restartButton.image
            .DOFade(1.0f, 1.0f)
            .From(0.0f)
            .SetDelay(1.5f)
            .onComplete += () => restartButton.image.raycastTarget = true;
    }
}
