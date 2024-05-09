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

    [SerializeField] SequenceGameSettings _gameSettings = null;

    SequenceHelper<int> _sequenceHelper;

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
        _currentSequence.Clear();

        Init();
        GenerateSequence();
    }

    public void Init()
    {
        _sequenceLength = _gameSettings.StartSequenceLength;
        _sequenceIncrement = _gameSettings.SequenceIncrementPerRound;

        // Set up RoundManager
        _roundManager.SetSettings(_gameSettings);
    }

    void GenerateSequence()
    {
        _sequenceHelper = new SequenceHelper<int>(_sequenceLength, _sequenceIncrement);
        _currentSequence = _sequenceHelper.GenerateSequence(() => UnityEngine.Random.Range(1, _amountOfCandles + 1));

        _candleVisualsController.ShowSequence(_currentSequence);
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

        float candleLitUpTime = _candleVisualsController.ShowSeparateCandle(input);
        
        if (_sequenceHelper.CheckSequenceInput(input))
        {
            if (_sequenceHelper.IsFinalSequenceInput())
            {
                MessageHub.Publish(new ChangeGameStateMessage(GameState.Cutscene));
                if (_roundManager.IsFinalRound())
                {
                    MessageHub.Publish(new ChangeGameStateMessage(GameState.Victory));
                    MessageHub.Publish(new EndGameMessage());
                }
                else
                {
                    // Wait for the candle to be unlit (after player input) before moving on to the next round
                    _candleLitTimer = Timer.Instance.AddTimer(candleLitUpTime, () => NextSequenceStep());
                }
            }
        }
        else
        {
            MessageHub.Publish(new ChangeGameStateMessage(GameState.Cutscene));
            _candleLitTimer = Timer.Instance.AddTimer(candleLitUpTime, () => RestartCurrentSequence());
        }
    }

    void NextSequenceStep()
    {
        AddToSequence();
        _roundManager.NextRound();
        _candleVisualsController.ShowSequence(_currentSequence);
    }

    void RestartCurrentSequence()
    {
        _sequenceHelper.ResetSequence();
        _candleVisualsController.ShowSequence(_currentSequence);
    }
}
