using Messaging;
using Messaging.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleVisualsController : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _candleRenderers = new List<SpriteRenderer>();

    Coroutine _seperateCandleCoroutine;

    const float _initialWaitTime = 1f;
    const float _DelayTime = .5f;
    const float _candleLitUpTime = 0.3f;

    public void ShowSequence(List<int> sequence)
    {
        StartCoroutine(ShowSequenceCoroutine(sequence));
    }

    IEnumerator ShowSequenceCoroutine(List<int> sequence)
    {
        ClearCandles();

        MessageHub.Publish(new ChangeGameStateMessage(GameState.Cutscene));
        yield return new WaitForSeconds(_initialWaitTime);
        for (int i = 0; i < sequence.Count; i++)
        {
            int candle = sequence[i] - 1;
            _candleRenderers[candle].enabled = true;
            yield return new WaitForSeconds(_candleLitUpTime);
            _candleRenderers[candle].enabled = false;
            yield return new WaitForSeconds(_DelayTime);
        }

        MessageHub.Publish(new ChangeGameStateMessage(GameState.Play));
    }

    public void ShowSeparateCandle(int candleIndex)
    {
        if (_seperateCandleCoroutine != null)
            StopCoroutine(_seperateCandleCoroutine);

        ClearCandles();

        _seperateCandleCoroutine = StartCoroutine(ShowCandleCoroutine(_candleRenderers[candleIndex - 1]));
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
}
