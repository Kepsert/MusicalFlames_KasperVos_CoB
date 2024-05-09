using Messaging;
using Messaging.Messages;
using UnityEngine;

public class RoundTimer : ITimer
{
    float _duration;
    public float Duration { get { return _duration; } set { _duration = value; } }

    float _elapsedTime;
    public float ElapsedTime { get { return _elapsedTime; } set { _elapsedTime = value; } }

    public RoundTimer(float duration)
    {
        _elapsedTime = 0;
        _duration = duration;
    }

    public void Reset()
    {
        _elapsedTime = 0;
    }

    public void Refresh()
    {
        ElapsedTime += Time.deltaTime;
        if (ElapsedTime >= Duration)
        {
            MessageHub.Publish(new EndGameMessage());
            MessageHub.Publish(new GameStateChangedMessage(GameState.Loss));
        }
    }
}
