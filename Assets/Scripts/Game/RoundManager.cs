using Messaging;
using Messaging.Messages;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    int _currentRound = 1;
    int _amountOfRounds = 3;

    void Start()
    {
        MessageHub.Subscribe<NewGameMessage>(this, NewGameStarted);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<NewGameMessage>(this);
    }

    public bool IsFinalRound()
    {
        if (_currentRound == _amountOfRounds)
            return true;
        else
            return false;
    }

    public void NextRound()
    {
        _currentRound++;
    }

    void NewGameStarted(NewGameMessage obj)
    {
        _currentRound = 1;
        _amountOfRounds = 3;
    }

}
