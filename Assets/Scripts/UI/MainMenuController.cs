using Messaging;
using Messaging.Messages;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuPanel = null;
    [SerializeField] SequenceGameSettings[] _gameSettings = null;
    
    public void StartGame(int buttonIndex)
    {
        _mainMenuPanel.SetActive(false);
        SequenceGameSettings gameSettings = _gameSettings[buttonIndex];
        if (gameSettings != null)
        {
            MessageHub.Publish(new NewGameMessage(gameSettings));
        }
        else
            Debug.LogWarning("No Game Settings Found");
    }

    public void PlaySound()
    {

    }
}
