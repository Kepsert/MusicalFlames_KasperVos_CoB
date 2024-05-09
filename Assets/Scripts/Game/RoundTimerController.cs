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

        MessageHub.Subscribe<InjectUIMessage>(this, InjectUIClass);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<EndGameMessage>(this);
        MessageHub.Unsubscribe<GameStateChangedMessage>(this);

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
            _roundTimerUI.UpdateValue(1);
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

    private void InjectUIClass(InjectUIMessage obj)
    {
        if (obj.Object is IUpdateable<float> iUpdateable)
        {
            _roundTimerUI = iUpdateable;
        }
    }
}
