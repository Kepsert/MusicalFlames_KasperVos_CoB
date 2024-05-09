using DG.Tweening;
using Messaging;
using Messaging.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputVisualsController : MonoBehaviour, IVisualsToggable
{
    [SerializeField] List<Transform> _inputVisualsList = new List<Transform>();
    Dictionary<Transform, Vector3> _inputVisualsStartPosition = new Dictionary<Transform, Vector3>();

    const float _inputVisualsStepDuration = 0.6f;

    void Start()
    {
        MessageHub.Subscribe<NewGameMessage>(this, StartNewGame);

        foreach (Transform inputVisual in _inputVisualsList)
        {
            _inputVisualsStartPosition.Add(inputVisual, inputVisual.position);
        }
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<NewGameMessage>(this);
    }

    public void ToggleVisualObject(bool toggle)
    {
        StartCoroutine(AnimateInputVisualsToggle(toggle));
    }

    IEnumerator AnimateInputVisualsToggle(bool toggle)
    {
        Vector3 moveDistance = toggle ? new Vector3(0, 3.5f, 0) : new Vector3(0, -3.5f, 0);
        MessageHub.Publish(new PlaySFXMessage("Woosh", 1*0.95f));
        foreach (Transform visual in _inputVisualsList)
        {
            Vector3 endPosition = visual.transform.position + moveDistance;
            visual.DOMove(endPosition, _inputVisualsStepDuration * 0.95f).SetEase(Ease.OutCirc);
            yield return new WaitForSeconds(0.075f);
        }
        yield return new WaitForSeconds(_inputVisualsStepDuration * 1.25f);

        if (toggle)
        {
            MessageHub.Publish(new ChangeGameStateMessage(GameState.Play));
        }
    }

    public float SwapInputVisuals(int amountOfCandles, InputValueHelper inputValueHelper)
    {
        List<int> indices = inputValueHelper.SwapTwoRandomValuesInList(1, amountOfCandles);
        List<Transform> inputVisualsToSwap = new List<Transform>(2);

        foreach (int i in indices)
        {
            inputVisualsToSwap.Add(_inputVisualsList[i]);
        }

        StartCoroutine(AnimateSwapVisuals(inputVisualsToSwap));

        return _inputVisualsStepDuration * 4;
    }

    IEnumerator AnimateSwapVisuals(List<Transform> visuals)
    {
        // Move visuals down
        Vector3 moveDistance = new Vector3(0, 0.65f);
        foreach (Transform visual in visuals)
        {
            Vector2 moveDownPosition = visual.transform.position - moveDistance;
            visual.transform.DOMove(moveDownPosition, _inputVisualsStepDuration * 0.95f).SetEase(Ease.OutBounce);
        }
        MessageHub.Publish(new PlaySFXMessage("Woosh", 1 * 0.95f));
        yield return new WaitForSeconds(_inputVisualsStepDuration);

        // Swap visuals' positions
        Vector2 item1Position = visuals[0].position;
        Vector2 item2Position = visuals[1].position;

        visuals[0].DOMove(item2Position, _inputVisualsStepDuration * 0.95f).SetEase(Ease.OutBounce);
        visuals[1].DOMove(item1Position, _inputVisualsStepDuration * 0.95f).SetEase(Ease.OutBounce);
        MessageHub.Publish(new PlaySFXMessage("Woosh", 1f));
        yield return new WaitForSeconds(_inputVisualsStepDuration);

        // Move visuals back to line
        foreach (Transform visual in visuals)
        {
            Vector2 moveUpPosition = visual.transform.position + moveDistance;
            visual.transform.DOMove(moveUpPosition, _inputVisualsStepDuration * 0.95f).SetEase(Ease.OutBounce);
        }
        MessageHub.Publish(new PlaySFXMessage("Woosh", 1 * 1.05f));
        yield return new WaitForSeconds(_inputVisualsStepDuration * 1.5f);
    }

    void StartNewGame(NewGameMessage obj)
    {
        ResetValues();
    }

    void ResetValues()
    {
        Vector3 position;
        foreach (var inputVisual in _inputVisualsList)
        {
            if (_inputVisualsStartPosition.TryGetValue(inputVisual, out position))
            {
                inputVisual.position = position;
            }
            else
            {
                Debug.LogWarning("Position not found for button " + inputVisual.name);
            }
        }
    }
}
