using DG.Tweening;
using Messaging;
using Messaging.Messages;
using System;
using System.Collections;
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

    const float _animationDuration = 1.75f;
    const float _moveDistance = 1750;

    bool _endlessMode = false;

    int _finalRound = 0;

    void Start()
    {
        MessageHub.Subscribe<GameStateChangedMessage>(this, GameStateChanged);
        MessageHub.Subscribe<GameModeChangedMessage>(this, GameModeChanged);
        MessageHub.Subscribe<RoundEndedMessage>(this, RoundEnded);

        _endGamePanel.transform.position += new Vector3(0, _moveDistance);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<GameStateChangedMessage>(this);
        MessageHub.Unsubscribe<GameModeChangedMessage>(this);
        MessageHub.Unsubscribe<RoundEndedMessage>(this);
    }

    void GameStateChanged(GameStateChangedMessage obj)
    {
        if (obj.GameState is GameState.Victory)
            GameResults(true);
        else if (obj.GameState is GameState.Loss)
            GameResults(false);
    }

    private void GameModeChanged(GameModeChangedMessage obj)
    {
        if (obj.GameMode == GameMode.Endless)
            _endlessMode = true;
    }

    void GameResults(bool win)
    {
        StartCoroutine(AnimateTogglePanelCoroutine(true));

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
            if (!_endlessMode)
            {
                _endGameImage.sprite = _endGameSprites[0];
                _endGameText.text = "Failed!";
                _endGameButtonText.text = "Try Again";
                MessageHub.Publish(new PlaySFXMessage("Fail"));
            }
            else
            {
                _endGameImage.sprite = _endGameSprites[1];
                _endGameText.text = "Round " + _finalRound;
                _endGameButtonText.text = "Play Again";
                MessageHub.Publish(new PlaySFXMessage("Success"));
            }
        }
    }

    public void EndGameButtonPressed()
    {
        StartCoroutine(AnimateTogglePanelCoroutine(false));

        MessageHub.Publish(new PlaySFXMessage("Button"));

        _endGameButton.interactable = false;

        MessageHub.Publish(new GameStateChangedMessage(GameState.Loading));
        MessageHub.Publish(new NewGameMessage());
    }

    IEnumerator AnimateTogglePanelCoroutine(bool toggle)
    {
        MessageHub.Publish(new PlaySFXMessage("Woosh"));

        Vector3 moveDistance = toggle ? new Vector3(0, -_moveDistance) : new Vector3(0, _moveDistance);
        Vector3 endPosition = _endGamePanel.transform.position + moveDistance;
        _endGamePanel.transform.DOMove(endPosition, _animationDuration).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(_animationDuration);

        if (!toggle)
        {
            _endGamePanel.SetActive(false);
        }
    }

    private void RoundEnded(RoundEndedMessage obj)
    {
        _finalRound = obj.RoundNumber;
    }
}
