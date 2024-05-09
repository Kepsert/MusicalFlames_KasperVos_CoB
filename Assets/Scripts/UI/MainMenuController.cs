using DG.Tweening;
using Messaging;
using Messaging.Messages;
using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuPanel = null;
    [SerializeField] SequenceGameSettings[] _gameSettings = null;

    const float _animationDuration = 1.25f;
    
    public void StartGame(int buttonIndex)
    {
        StartCoroutine(AnimatePanelCoroutine(buttonIndex));
    }

    IEnumerator AnimatePanelCoroutine(int index)
    {
        Vector3 endPosition = _mainMenuPanel.transform.position + new Vector3(0, -1500, 0);
        _mainMenuPanel.transform.DOMove(endPosition, _animationDuration * 0.95f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(_animationDuration);

        _mainMenuPanel.SetActive(false);
        LaunchGame(index);
    } 

    void LaunchGame(int buttonIndex)
    {
        SequenceGameSettings gameSettings = _gameSettings[buttonIndex];

        if (buttonIndex == 3)
            MessageHub.Publish(new GameModeChangedMessage(GameMode.Endless));

        if (gameSettings != null)
        {
            MessageHub.Publish(new NewGameMessage(gameSettings));
        }
        else
            Debug.LogWarning("No Game Settings Found");
    }

    public void PlaySound()
    {
        MessageHub.Publish(new PlaySFXMessage("Button"));
    }
}
