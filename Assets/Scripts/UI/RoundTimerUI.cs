using DG.Tweening;
using Messaging;
using Messaging.Messages;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimerUI : MonoBehaviour, IUpdateable<float>
{
    [SerializeField] Slider _slider = null;

    const float _animationDuration = .75f;
    const float _moveDistance = 150;

    Coroutine _resetValueOverTime;

    void Start()
    {
        MessageHub.Subscribe<NewGameMessage>(this, NewGameStarted);
        MessageHub.Subscribe<EndGameMessage>(this, GameEnded);

        _slider.transform.position += new Vector3(0, _moveDistance);
    }

    private void OnDestroy()
    {
        MessageHub.Unsubscribe<NewGameMessage>(this);
        MessageHub.Unsubscribe<EndGameMessage>(this);
    }

    void NewGameStarted(NewGameMessage obj)
    {
        _slider.gameObject.SetActive(true);

        StartCoroutine(AnimateToggleCoroutine(true));

        MessageHub.Publish(new InjectUIMessage(this));
    }

    void GameEnded(EndGameMessage obj)
    {
        StartCoroutine(AnimateToggleCoroutine(false));
    }

    IEnumerator AnimateToggleCoroutine(bool activate)
    {
        Vector3 moveDistance = new Vector2(0, _moveDistance);
        Vector3 endPosition = activate ? _slider.gameObject.transform.position - moveDistance : _slider.transform.position + moveDistance;

        _slider.transform.DOMove(endPosition, _animationDuration * 0.95f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(_animationDuration);

        if (!activate)
            _slider.gameObject.SetActive(false);
    }

    public void UpdateValue(float value)
    {
        _slider.value = 1 - value;
    }

    /// <summary>
    /// Gradually fill the timer back up
    /// </summary>
    /// <param name="duration"></param>
    public void RefreshOverTime(float duration)
    {
        _resetValueOverTime = StartCoroutine(ResetValueOverTimeCoroutine(duration));
    }

    IEnumerator ResetValueOverTimeCoroutine(float duration)
    {
        float timer = 0f;
        float startValue = _slider.value;
        float endValue = 1f;

        while (timer < duration)
        {
            _slider.value = Mathf.Lerp(startValue, endValue, timer / duration);

            timer += Time.deltaTime;

            yield return null;
        }

        _slider.value = endValue;
    }
}
