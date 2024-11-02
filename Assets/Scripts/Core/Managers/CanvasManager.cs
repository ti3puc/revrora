using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Managers;
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

        public static DialogManager DialogCanvas => Instance._dialogCanvas;
        public static CanvasGroup TransitionCanvasGroup => Instance._transitionCanvasGroup;

        protected override void Awake()
        {
            base.Awake();
            PlayerInput.OnToggleInventoryStarted += ToggleInventory;
        }

        private void OnDestroy()
        {
            PlayerInput.OnToggleInventoryStarted -= ToggleInventory;
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
    }
}
