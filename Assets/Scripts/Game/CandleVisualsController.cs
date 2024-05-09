using Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleVisualsController : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _candleRenderers = new List<SpriteRenderer>();

    Coroutine _seperateCandleCoroutine;

    const float _initialWaitTime = 1f;
    const float _DelayTime = .7f;
    const float _candleLitUpTime = 0.3f;

    public void ShowSequence(List<int> sequence)
    {
        StartCoroutine(ShowSequenceCoroutine(sequence));
    }

    IEnumerator ShowSequenceCoroutine(List<int> sequence)
    {
        GameController.Instance.SetGameState(GameState.Cutscene);
        yield return new WaitForSeconds(_initialWaitTime);
        for (int i = 0; i < sequence.Count; i++)
        {
            int candle = sequence[i] - 1;
            _candleRenderers[candle].enabled = true;
            yield return new WaitForSeconds(_candleLitUpTime);
            _candleRenderers[candle].enabled = false;
            yield return new WaitForSeconds(_DelayTime);
        }
    }
}