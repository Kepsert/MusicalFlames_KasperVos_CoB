using Messaging;
using Messaging.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleVisualsController : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _candleRenderers = new List<SpriteRenderer>();

    Coroutine _seperateCandleCoroutine;
    Coroutine _showSequenceCoroutine;

    // The time it takes before showing the sequence
    const float _initialWaitTime = 1f;
    // The time it takes between each sequence item
    const float _DelayTime = .5f;
    // The time each sequence item will be active
    const float _candleLitUpTime = 0.3f;

    void Start()
    {
        MessageHub.Subscribe<EndGameMessage>(this, GameEnded);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<EndGameMessage>(this);
    }

    /// <summary>
    /// Show the entire sequence to the player
    /// </summary>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public float ShowSequence(List<int> sequence)
    {
        _showSequenceCoroutine = StartCoroutine(ShowSequenceCoroutine(sequence));

        // Return the amount of time the full coroutine will take
        return _initialWaitTime + ((_candleLitUpTime + _DelayTime) * sequence.Count);
    }

    IEnumerator ShowSequenceCoroutine(List<int> sequence)
    {
        ClearCandles();

        MessageHub.Publish(new ChangeGameStateMessage(GameState.Cutscene));
        yield return new WaitForSeconds(_initialWaitTime);
        for (int i = 0; i < sequence.Count; i++)
        {
            int candle = sequence[i] - 1;

            float pitch = (1 - .1f) + (float)candle * 0.05f;
            MessageHub.Publish(new PlaySFXMessage("Ring", pitch));

            _candleRenderers[candle].enabled = true;
            yield return new WaitForSeconds(_candleLitUpTime);
            _candleRenderers[candle].enabled = false;
            yield return new WaitForSeconds(_DelayTime);
        }
    }

    /// <summary>
    /// Light up a single candle, such as when the player presses a key
    /// </summary>
    /// <param name="candleIndex"></param>
    /// <returns></returns>
    public float ShowSeparateCandle(int candleIndex)
    {
        if (_seperateCandleCoroutine != null)
            StopCoroutine(_seperateCandleCoroutine);

        ClearCandles();

        _seperateCandleCoroutine = StartCoroutine(ShowCandleCoroutine(_candleRenderers[candleIndex - 1]));

        return _candleLitUpTime;
    }

    IEnumerator ShowCandleCoroutine(SpriteRenderer candle)
    {
        candle.enabled = true;
        yield return new WaitForSeconds(_candleLitUpTime);
        candle.enabled = false;
    }

    void ClearCandles()
    {
        foreach (SpriteRenderer renderer in _candleRenderers)
        {
            renderer.enabled = false;
        }
    }

    void GameEnded(EndGameMessage obj)
    {
        StopCoroutine(_showSequenceCoroutine);
        ClearCandles();
    }
}
