using Messaging;
using Messaging.Messages;
using UnityEngine;

public enum GameState { Menu, Loading, Play, Cutscene, Victory, Loss }
public enum GameMode { Normal, Endless }

public class GameController : MonoBehaviour
{
    [SerializeField] bool _testRun = false;

    [field: SerializeField] public GameState State { get; private set; } = GameState.Menu;
    public void SetGameState(GameState state)
    {
        State = state;
        MessageHub.Publish(new GameStateChangedMessage(State));
    }

    [field: SerializeField] public GameMode Mode { get; private set; } = GameMode.Normal;
    public void SetGameMode(GameMode mode)
    {
        Mode = mode;
        MessageHub.Publish(new GameModeChangedMessage(Mode));
    }

    void Start()
    {
        MessageHub.Subscribe<ChangeGameStateMessage>(this, ChangeGameState);

        if (_testRun)
        {
            Invoke("TestInitialization", 1f);
        }
    }

    void TestInitialization()
    {
        MessageHub.Publish(new NewGameMessage());
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<ChangeGameStateMessage>(this);
    }

    private void ChangeGameState(ChangeGameStateMessage obj)
    {
        SetGameState(obj.State);
    }
}
