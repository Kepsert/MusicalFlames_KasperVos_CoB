using Messaging;
using Messaging.Messages;
using UnityEngine;

public class RoundTimerController : MonoBehaviour
{
    ITimer _roundTimer = null;
    float _roundTime;

    bool _timerRunning = false;

    IUpdateable<float> _roundTimerUI = null;

    void Start()
    {
        MessageHub.Subscribe<EndGameMessage>(this, GameEnded);
        MessageHub.Subscribe<GameStateChangedMessage>(this, GameStateChanged);

        MessageHub.Subscribe<RoundEndedMessage>(this, RoundEnded);

        MessageHub.Subscribe<InjectUIMessage>(this, InjectUIClass);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<EndGameMessage>(this);
        MessageHub.Unsubscribe<GameStateChangedMessage>(this);

        MessageHub.Unsubscribe<RoundEndedMessage>(this);

        MessageHub.Subscribe<InjectUIMessage>(this, InjectUIClass);
    }

    void Update()
    {
        if (_timerRunning)
        {
            _roundTimer.Refresh();
            float fillAmount = _roundTimer.ElapsedTime / _roundTimer.Duration;
            if (_roundTimerUI != null)
                _roundTimerUI.UpdateValue(fillAmount);
        }
    }

    /// <summary>
    /// When the game starts, set the starting values for the controller
    /// </summary>
    /// <param name="roundTime"></param>
    public void GameStarted(float roundTime)
    {
        _roundTime = roundTime;

        _roundTimer = new RoundTimer(_roundTime);
    }

    void GameEnded(EndGameMessage obj)
    {
        _timerRunning = false;
        _roundTimer.Reset();
        if (_roundTimerUI != null)
            _roundTimerUI.RefreshOverTime(0.1f);
    }

    void GameStateChanged(GameStateChangedMessage obj)
    {
        if (obj.GameState == GameState.Play)
        {
            _timerRunning = true;
        }
        else
        {
            _timerRunning = false;
        }
    }

    /// <summary>
    /// Use an event to inject required dependencies into this script
    /// </summary>
    /// <param name="obj"></param>
    private void InjectUIClass(InjectUIMessage obj)
    {
        if (obj.Object is IUpdateable<float> iUpdateable)
        {
            _roundTimerUI = iUpdateable;
        }
    }

    private void RoundEnded(RoundEndedMessage obj)
    {
        _roundTimer.Reset();
        if (_roundTimerUI != null)
            _roundTimerUI.RefreshOverTime(1);
    }
}
