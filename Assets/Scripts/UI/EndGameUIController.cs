using Messaging;
using Messaging.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour
{
    [SerializeField] GameObject _endGamePanel = null;
    [SerializeField] Button _endGameButton = null;
    [SerializeField] TMP_Text _endGameText = null;
    [SerializeField] TMP_Text _endGameButtonText = null;
    [SerializeField] Image _endGameImage = null;
    [SerializeField] Sprite[] _endGameSprites = null;

    void Start()
    {
        MessageHub.Subscribe<GameStateChangedMessage>(this, GameStateChanged);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<GameStateChangedMessage>(this);
    }

    void GameStateChanged(GameStateChangedMessage obj)
    {
        if (obj.GameState is GameState.Victory)
            GameResults(true);
        else if (obj.GameState is GameState.Loss)
            GameResults(false);
    }

    void GameResults(bool win)
    {
        _endGamePanel.SetActive(true);
        _endGameButton.interactable = true;
        if (win)
        {
            _endGameImage.sprite = _endGameSprites[1];
            _endGameText.text = "Good Job!";
            _endGameButtonText.text = "Play Again";
            MessageHub.Publish(new PlaySFXMessage("Success"));
        }
        else
        {
            _endGameImage.sprite = _endGameSprites[0];
            _endGameText.text = "Failed!";
            _endGameButtonText.text = "Try Again";
            MessageHub.Publish(new PlaySFXMessage("Fail"));
        }
    }

    public void EndGameButtonPressed()
    {
        _endGameButton.interactable = false;

        _endGamePanel.SetActive(false);
        MessageHub.Publish(new GameStateChangedMessage(GameState.Loading));
        MessageHub.Publish(new NewGameMessage());
    }
}
