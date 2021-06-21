using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] private ParticleShooter particleShooter;

    [HideInInspector] public StringUnityEvent answerPickedEvent;
    [HideInInspector] public BoolUnityEvent answerEvaluatedEvent;

    private string correctAnswer;

    private bool isInit;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (isInit)
            return;

        GameController.Instance.levelStartedEvent.AddListener(OnLevelStarted);
        answerPickedEvent.AddListener(OnAnswerPicked);

        isInit = true;
    }

    public void InitializeLevel(string correctAnswer)
    {
        Initialize();

        this.correctAnswer = correctAnswer;
    }

    private void OnLevelStarted(string correctAnswer)
    {
        InitializeLevel(correctAnswer);
    }

    private void OnAnswerPicked(string name)
    {
        if (correctAnswer == name)
            particleShooter.Shoot();

        answerEvaluatedEvent.Invoke(correctAnswer == name);
    }
}
