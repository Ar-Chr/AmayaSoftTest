using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameController : Singleton<GameController>
{
    [SerializeField] private List<DifficultyLevel> difficultyLevels;
    [Space]
    [SerializeField] private List<ItemSet_SO> itemSets;
    [SerializeField] private LevelBuilder levelBuilder;

    [HideInInspector] public UnityEvent gameStartedEvent;
    [HideInInspector] public StringUnityEvent levelStartedEvent;
    [HideInInspector] public UnityEvent gameFinishedEvent;

    private int currentDifficulty;

    private Dictionary<ItemSet_SO, List<Item>> availableCorrectAnswers;

    private bool isInit;

    private void Start()
    {
        Initialize();
        StartCoroutine(DelayAction(StartNewGame, 0.2f));
    }

    private void Initialize()
    {
        if (isInit)
            return;

        availableCorrectAnswers = itemSets.ToDictionary(set => set, set => set.items.ToList());

        LevelController.Instance.answerEvaluatedEvent.AddListener(OnAnswerEvaluated);
        UiController.Instance.restartButtonClickedEvent.AddListener(StartNewGame);

        isInit = true;
    }

    private void StartNewGame()
    {
        currentDifficulty = 0;
        StartLevelAnimated((cell, index) => cell.Bounce());
        gameStartedEvent.Invoke();
    }

    private void StartLevelAnimated(Action<GridCell, int> animation)
    {
        var difficulty = difficultyLevels[currentDifficulty];
        var itemSet = itemSets[Random.Range(0, itemSets.Count)];

        var answerItems = availableCorrectAnswers[itemSet];
        var correctAnswer = answerItems[Random.Range(0, answerItems.Count)];
        availableCorrectAnswers[itemSet].Remove(correctAnswer);
        if (availableCorrectAnswers[itemSet].Count == 0)
        {
            availableCorrectAnswers[itemSet].AddRange(itemSet.items);
        }

        var randomItems = GenerateRandomItems(correctAnswer, difficulty.TotalCells, itemSet);
        levelBuilder.BuildAnimated(randomItems, difficulty.Columns, animation);
        levelStartedEvent.Invoke(correctAnswer.Name);
    }

    private void StartLevel()
    {
        StartLevelAnimated((cell, index) => { });
    }

    private List<Item> GenerateRandomItems(Item correctAnswer, int count, ItemSet_SO itemSet)
    {
        var items = new List<Item>(count);

        var itemTaken = new bool[itemSet.items.Count];

        var correctAnswerPosition = Random.Range(0, count);
        var correctAnswerIndex = itemSet.items.FindIndex(i => i.Name == correctAnswer.Name);
        itemTaken[correctAnswerIndex] = true;

        for (int i = 0; i < count; i++)
        {
            if (i == correctAnswerPosition)
            {
                items.Add(correctAnswer);
                continue;
            }

            int numberInSet = Random.Range(0, itemSet.items.Count);
            while (itemTaken[numberInSet])
            {
                numberInSet = ++numberInSet == itemSet.items.Count ? 0 : numberInSet;
            }
            items.Add(itemSet.items[numberInSet]);
            itemTaken[numberInSet] = true;
        }

        return items;
    }

    private void Finish()
    {
        gameFinishedEvent.Invoke();
    }

    private void OnAnswerEvaluated(bool correct)
    {
        if (correct)
        {
            currentDifficulty++;
            if (currentDifficulty == difficultyLevels.Count)
            {
                Finish();
            }
            else
            {
                StartCoroutine(DelayAction(StartLevel, 1.5f));
            }
        }
    }

    public static IEnumerator DelayAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
