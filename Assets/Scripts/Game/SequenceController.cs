using Messaging;
using Messaging.Messages;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    [SerializeField] CandleVisualsController _candleVisualsController = null;

    SequenceHelper<int> _sequenceHelper;

    List<int> _currentSequence = new List<int>();

    const int _amountOfCandles = 5;

    int _currentRound = 1;
    int _sequenceLength = 3;
    int _sequenceIncrement = 1;
    int _amountOfRounds = 4;

    private void Awake()
    {
        if (_candleVisualsController == null)
            _candleVisualsController = GetComponent<CandleVisualsController>();
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
        _currentRound = 1;
        _sequenceLength = 3;
        _sequenceIncrement = 1;
        _amountOfRounds = 4;
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
            _sequenceHelper.AddToSequence(() => UnityEngine.Random.Range(1, _amountOfCandles + 1));
        }
        else
        {
            Debug.LogWarning("There is currently no existing sequencehelper, so no sequence items can be added.");
        }
    }

    public void CompareInputWithSequence(int input)
    {
        if (_sequenceHelper.CheckSequenceInput(input))
        {
            if (_sequenceHelper.IsFinalSequenceInput())
            {
            }
        }
        else
        {
            _sequenceHelper.ResetSequence();
        }
    }
}
