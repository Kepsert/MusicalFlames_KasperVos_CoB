using Messaging;
using Messaging.Messages;
using Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    [SerializeField] CandleVisualsController _candleVisualsController = null;
    [SerializeField] RoundManager _roundManager = null;
    [SerializeField] InputVisualsController _inputVisualsController = null;
    [SerializeField] RoundTimerController _roundTimerController = null;

    [SerializeField] SequenceGameSettings _gameSettings = null;

    SequenceHelper<int> _sequenceHelper;
    InputValueHelper _inputValueHelper;

    List<int> _currentSequence = new List<int>();

    const int _amountOfCandles = 5;

    int _sequenceLength = 3;
    int _sequenceIncrement = 1;

    Guid _candleLitTimer;

    private void Awake()
    {
        if (_candleVisualsController == null)
            _candleVisualsController = GetComponent<CandleVisualsController>();
        if (_roundManager == null)
            _roundManager = GetComponent<RoundManager>();
        if (_inputVisualsController == null)
            _inputVisualsController = GetComponent<InputVisualsController>();
        if (_roundTimerController == null)
            _roundTimerController = GetComponent<RoundTimerController>();
    }

    void Start()
    {
        MessageHub.Subscribe<NewGameMessage>(this, NewGameStarted);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<NewGameMessage>(this);
    }

    void NewGameStarted(NewGameMessage obj)
    {
        if (obj.SequenceGameSettings != null)
            _gameSettings = obj.SequenceGameSettings;

        _currentSequence.Clear();

        Init();
        GenerateSequence();
    }

    public void Init()
    {
        _sequenceLength = _gameSettings.StartSequenceLength;
        _sequenceIncrement = _gameSettings.SequenceIncrementPerRound;
        _roundTimerController.GameStarted(_gameSettings.RoundTimer);

        // Set up RoundManager
        _roundManager.SetSettings(_gameSettings);
    }

    void GenerateSequence()
    {
        _inputValueHelper = new InputValueHelper(_amountOfCandles);

        _sequenceHelper = new SequenceHelper<int>(_sequenceLength, _sequenceIncrement);
        _currentSequence = _sequenceHelper.GenerateSequence(() => UnityEngine.Random.Range(1, _amountOfCandles + 1));

        ShowSequence();
    }

    void AddToSequence(int amount = 0)
    {
        if (_sequenceHelper != null)
        {
            _currentSequence = _sequenceHelper.AddToSequence(() => UnityEngine.Random.Range(1, _amountOfCandles + 1));
        }
        else
        {
            Debug.LogWarning("There is currently no existing sequencehelper, so no sequence items can be added.");
        }
    }

    public void CompareInputWithSequence(int input)
    {
        if (_candleLitTimer != null)
            Timer.Instance.RemoveTimer(_candleLitTimer);

        int inputValue = _inputValueHelper.GetInputValue(input);

        float candleLitUpTime = _candleVisualsController.ShowSeparateCandle(inputValue);

        if (_sequenceHelper.CheckSequenceInput(inputValue))
        {
            if (_sequenceHelper.IsFinalSequenceInput())
            {
                MessageHub.Publish(new ChangeGameStateMessage(GameState.Cutscene));
                if (_roundManager.IsFinalRound())
                {
                    InitiateVictory();
                }
                else
                {
                    // Wait for the candle to be unlit (after player input) before moving on to the next round
                    _candleLitTimer = Timer.Instance.AddTimer(candleLitUpTime, () => NextRound());
                }
            }
        }
        else
        {
            MessageHub.Publish(new ChangeGameStateMessage(GameState.Cutscene));
            ToggleInputVisuals(false);
            _candleLitTimer = Timer.Instance.AddTimer(candleLitUpTime, () => RestartCurrentSequence());
        }
    }

    void NextRound()
    {
        AddToSequence();
        _roundManager.NextRound();
        if (_roundManager.SwapBlockRound())
        {
            float swapInputValueDuration = _inputVisualsController.SwapInputVisuals(_amountOfCandles, _inputValueHelper);

            // Wait for the input visuals to be swapped before moving on to the next sequence
            Timer.Instance.AddTimer(swapInputValueDuration, () =>
            {
                ToggleInputVisuals(false);
                ShowSequence();
            });
        }
        else
        {
            ToggleInputVisuals(false);
            ShowSequence();
        }
    }

    void RestartCurrentSequence()
    {
        _sequenceHelper.ResetSequence();
        ShowSequence();
    }

    void ShowSequence()
    {
        float showSequenceDuration = _candleVisualsController.ShowSequence(_currentSequence);

        // When sequence has been shown, bring up the input visuals
        Timer.Instance.AddTimer(showSequenceDuration, () =>
        {
            ToggleInputVisuals(true);
        });
    }

    void ToggleInputVisuals(bool toggle)
    {
        _inputVisualsController.ToggleVisualObject(toggle);
    }

    void InitiateVictory()
    {
        ToggleInputVisuals(false);
        MessageHub.Publish(new ChangeGameStateMessage(GameState.Victory));
        MessageHub.Publish(new EndGameMessage());
    }
}
