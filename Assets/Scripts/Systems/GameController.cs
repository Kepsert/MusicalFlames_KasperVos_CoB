using Messaging;
using Messaging.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Menu, Loading, Play, Cutscene, Victory, Loss }
public enum GameMode { Normal, Endless }

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

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
}
