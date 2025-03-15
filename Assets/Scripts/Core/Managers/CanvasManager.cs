using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Environment.Box;
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
        [SerializeField] private Canvas _boxCanvas;
        [SerializeField] private Canvas _pauseCanvas;
        [SerializeField] private Canvas _storeCanvas;
        [SerializeField] private Canvas _dialogOptionsCanvas;
        [SerializeField] private CanvasGroup _transitionCanvasGroup;
        [SerializeField] private DialogManager _dialogCanvas;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isInventoryOpen;
        [SerializeField, ReadOnly] private bool _isPaused;
        [SerializeField, ReadOnly] private bool _isBoxOpen;
        [SerializeField, ReadOnly] private bool _isStoreOpen;
        [SerializeField, ReadOnly] private bool _isDialogOptionsOpen;

        public static DialogManager DialogCanvas => Instance._dialogCanvas;
        public static CanvasGroup TransitionCanvasGroup => Instance._transitionCanvasGroup;

        protected override void Awake()
        {
            base.Awake();
            PlayerInput.OnToggleInventoryStarted += ToggleInventory;
            PlayerInput.OnPauseStarted += TogglePause;
            BoxAccess.OnBoxAccessed += ToggleDialogOptions;
            BoxAccess.OnBoxExit += CloseBox;
        }

        private void OnDestroy()
        {
            PlayerInput.OnToggleInventoryStarted -= ToggleInventory;
            PlayerInput.OnPauseStarted -= TogglePause;
            BoxAccess.OnBoxAccessed -= ToggleDialogOptions;
            BoxAccess.OnBoxExit -= CloseBox;
        }

        private void Start()
        {
            ShowMainCanvas();
        }

        public void ShowMainCanvas()
        {
            if (_mainCanvas == null) return;

            _mainCanvas.gameObject.SetActive(true);
            _inventoryCanvas.gameObject.SetActive(false);
            _pauseCanvas.gameObject.SetActive(false);
            _boxCanvas.gameObject.SetActive(false);
            _storeCanvas.gameObject.SetActive(false);
            _dialogOptionsCanvas.gameObject.SetActive(false);

            if (_isInventoryOpen)
                _isInventoryOpen = !_isInventoryOpen;

            if (_isBoxOpen)
                _isBoxOpen = !_isBoxOpen;
        }

        public void ToggleInventory()
        {
            _isInventoryOpen = !_isInventoryOpen;

            _mainCanvas.gameObject.SetActive(!_isInventoryOpen);
            _inventoryCanvas.gameObject.SetActive(_isInventoryOpen);
            _pauseCanvas.gameObject.SetActive(false);
            _boxCanvas.gameObject.SetActive(false);
            _storeCanvas.gameObject.SetActive(false);
            _dialogOptionsCanvas.gameObject.SetActive(false);

            if (_isBoxOpen)
                _isBoxOpen = !_isBoxOpen;
        }

        public void TogglePause()
        {
            if (_isInventoryOpen)
            {
                ToggleInventory();
                return;
            }

            if (_isBoxOpen)
            {
                ToggleStorage();
                return;
            }

            _isPaused = !_isPaused;

            _mainCanvas.gameObject.SetActive(false);
            _inventoryCanvas.gameObject.SetActive(false);
            _boxCanvas.gameObject.SetActive(false);
            _pauseCanvas.gameObject.SetActive(_isPaused);
            _storeCanvas.gameObject.SetActive(false);
            _dialogOptionsCanvas.gameObject.SetActive(false);

            if (_isPaused)
                GameManager.Pause();
            else
            {
                GameManager.Resume();
                ShowMainCanvas();
            }
        }

        public void ToggleStorage()
        {
            _isBoxOpen = !_isBoxOpen;

            _mainCanvas.gameObject.SetActive(!_isBoxOpen);
            _inventoryCanvas.gameObject.SetActive(false);
            _pauseCanvas.gameObject.SetActive(false);
            _boxCanvas.gameObject.SetActive(_isBoxOpen);
            _storeCanvas.gameObject.SetActive(false);
            _dialogOptionsCanvas.gameObject.SetActive(false);
        }

        private void CloseBox()
        {
            if (_isBoxOpen)
                ToggleStorage();
            if (_isDialogOptionsOpen)
                ToggleDialogOptions();
            if (_isStoreOpen)
                ToggleStore();
        }

        public void ToggleStore()
        {
            _isStoreOpen = !_isStoreOpen;

            _mainCanvas.gameObject.SetActive(!_isStoreOpen);
            _inventoryCanvas.gameObject.SetActive(false);
            _pauseCanvas.gameObject.SetActive(false);
            _boxCanvas.gameObject.SetActive(false);
            _storeCanvas.gameObject.SetActive(_isStoreOpen);
            _dialogOptionsCanvas.gameObject.SetActive(false);
        }

        public void ToggleDialogOptions()
        {
            _isDialogOptionsOpen = !_isDialogOptionsOpen;

            _mainCanvas.gameObject.SetActive(!_isDialogOptionsOpen);
            _inventoryCanvas.gameObject.SetActive(false);
            _pauseCanvas.gameObject.SetActive(false);
            _boxCanvas.gameObject.SetActive(false);
            _storeCanvas.gameObject.SetActive(false);
            _dialogOptionsCanvas.gameObject.SetActive(_isDialogOptionsOpen);
        }

        public void GoToMenu()
        {
            GameManager.Resume();
            ScenesManager.LoadScene("Main Menu");
        }
    }
}
