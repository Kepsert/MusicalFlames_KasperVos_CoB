using Messaging;
using Messaging.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput _playerInput;
    InputAction _toggleCandleAction;

    bool _hasControl = false;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput != null)
        {
            _toggleCandleAction = _playerInput.currentActionMap.FindAction("ToggleCandle");
        }
        else
        {
            Debug.LogWarning("PlayerInput component missing on " + this.name);
        }
    }

    void Start()
    {
        MessageHub.Subscribe<GameStateChangedMessage>(this, GameStateChanged);

        _toggleCandleAction.performed += ToggleCandleButtonPressed;
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<GameStateChangedMessage>(this);

        _toggleCandleAction.performed -= ToggleCandleButtonPressed;
    }

    private void GameStateChanged(GameStateChangedMessage obj)
    {
        if (obj.GameState == GameState.Play)
            _hasControl = true;
        else
            _hasControl = false;
    }

    private void ToggleCandleButtonPressed(InputAction.CallbackContext obj)
    {
        if (_hasControl)
        {
            int value = (int)obj.ReadValue<float>();
        }
    }
}
