using Messaging;
using Messaging.Messages;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    SequenceGameSettings _gameSettings = null;

    int _currentRound = 1;
    int _amountOfRounds = 3;

    public void SetSettings(SequenceGameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        ResetGame();
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

    void ResetGame()
    {
        _currentRound = 1;
        _amountOfRounds = _gameSettings.AmountOfRounds;
    }
}
