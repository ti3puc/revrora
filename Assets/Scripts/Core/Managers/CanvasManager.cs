using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Managers;
using Managers.Scenes;
using NaughtyAttributes;
using Player.Input;
using UnityEngine;

namespace UI
{
    public class CanvasManager : Singleton<CanvasManager>
    {
        [Header("References")]
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Canvas _inventoryCanvas;
        [SerializeField] private Canvas _pauseCanvas;
        [SerializeField] private CanvasGroup _transitionCanvasGroup;
        [SerializeField] private DialogManager _dialogCanvas;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isInventoryOpen;
        [SerializeField, ReadOnly] private bool _isPauseOpen;

        public static DialogManager DialogCanvas => Instance._dialogCanvas;
        public static CanvasGroup TransitionCanvasGroup => Instance._transitionCanvasGroup;

        protected override void Awake()
        {
            base.Awake();
            PlayerInput.OnToggleInventoryStarted += ToggleInventory;
            PlayerInput.OnPauseStarted += TogglePause;
        }

        private void OnDestroy()
        {
            PlayerInput.OnToggleInventoryStarted -= ToggleInventory;
            PlayerInput.OnPauseStarted -= TogglePause;
        }

        public void ShowMainCanvas()
        {
            _mainCanvas.enabled = true;
            _inventoryCanvas.enabled = false;
            _pauseCanvas.enabled = false;

            if (_isInventoryOpen)
                _isInventoryOpen = !_isInventoryOpen;
        }

        public void ToggleInventory()
        {
            _isInventoryOpen = !_isInventoryOpen;

            _mainCanvas.enabled = !_isInventoryOpen;
            _inventoryCanvas.enabled = _isInventoryOpen;
            _pauseCanvas.enabled = false;
        }

        public void TogglePause()
        {
            _mainCanvas.enabled = false;
            _inventoryCanvas.enabled = false;
            _pauseCanvas.enabled = !_pauseCanvas.enabled;

            if (_pauseCanvas.enabled)
                GameManager.Pause();
            else
            {
                GameManager.Resume();
                ShowMainCanvas();
            }
        }

        public void GoToMenu()
        {
            ScenesManager.LoadScene("Main Menu");
        }
    }
}
