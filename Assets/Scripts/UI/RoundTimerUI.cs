using Messaging;
using Messaging.Messages;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimerUI : MonoBehaviour, IUpdateable<float>
{
    [SerializeField] Slider _slider = null;

    void Start()
    {
        MessageHub.Subscribe<NewGameMessage>(this, NewGameStarted);
        MessageHub.Subscribe<EndGameMessage>(this, GameEnded);
    }

    private void OnDestroy()
    {
        MessageHub.Unsubscribe<NewGameMessage>(this);
        MessageHub.Unsubscribe<EndGameMessage>(this);
    }

    void NewGameStarted(NewGameMessage obj)
    {
        _slider.gameObject.SetActive(true);

        MessageHub.Publish(new InjectUIMessage(this));
    }

    void GameEnded(EndGameMessage obj)
    {
        _slider.gameObject.SetActive(false);
    }

    public void UpdateValue(float value)
    {
        _slider.value = 1 - value;
    }
}
