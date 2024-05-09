using Messaging;
using Messaging.Messages;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    SequenceGameSettings _gameSettings = null;

    int _currentRound = 1;
    int _amountOfRounds = 3;
    int _swapBlockStartRound = 5;

    bool _endlessMode = false;

    public void SetSettings(SequenceGameSettings gameSettings, bool endlessMode)
    {
        _gameSettings = gameSettings;
        _endlessMode = endlessMode;
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
        if (_endlessMode)
            MessageHub.Publish(new RoundEndedMessage());
    }

    /// <summary>
    /// Is it time to introduce the swapping block mechanic
    /// </summary>
    /// <returns></returns>
    public bool SwapBlockRound()
    {
        if (_currentRound >= _swapBlockStartRound)
        {
            return true;
        }
        return false;
    }

    void ResetGame()
    {
        _currentRound = 1;
        _amountOfRounds = _gameSettings.AmountOfRounds;
        _swapBlockStartRound = _gameSettings.SwapBlockStartRound;
    }
}
