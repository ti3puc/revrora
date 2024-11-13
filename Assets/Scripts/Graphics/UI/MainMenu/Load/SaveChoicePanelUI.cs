using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Load
{
    public class SaveChoicePanelUI : MonoBehaviour
    {
        public enum ChoiceType
        {
            New,
            Load,
            Delete
        }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private ChoiceType _choiceType;

        [Header("Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        public static bool IsAnyPanelActive { get; private set; }

        private Action _yesCallback;
        private Action _noCallback;

        private void Awake()
        {
            if (_choiceType == ChoiceType.New)
                SaveSlotUI.OnSlotNewSaveSelected += Show;
            else if (_choiceType == ChoiceType.Load)
                SaveSlotUI.OnSlotLoadSaveSelected += Show;
            else if (_choiceType == ChoiceType.Delete)
                SaveSlotUI.OnSlotDeleteSaveSelected += Show;

            _yesButton.onClick.AddListener(CallYesCallback);
            _noButton.onClick.AddListener(CallNoCallback);
        }

        private void OnDestroy()
        {
            if (_choiceType == ChoiceType.New)
                SaveSlotUI.OnSlotNewSaveSelected -= Show;
            else if (_choiceType == ChoiceType.Load)
                SaveSlotUI.OnSlotLoadSaveSelected -= Show;
            else if (_choiceType == ChoiceType.Delete)
                SaveSlotUI.OnSlotDeleteSaveSelected -= Show;

            _yesButton.onClick.RemoveListener(CallYesCallback);
            _noButton.onClick.RemoveListener(CallNoCallback);
        }

        public void Show(Action yesCallback, Action noCallback)
        {
            if (IsAnyPanelActive) return;

            _yesCallback = yesCallback;
            _noCallback = noCallback;

            _canvas.enabled = true;
            IsAnyPanelActive = true;
        }

        public void Hide()
        {
            if (IsAnyPanelActive == false) return;

            _canvas.enabled = false;
            IsAnyPanelActive = false;
        }

        private void CallYesCallback()
        {
            _yesCallback?.Invoke();
            Hide();
        }

        private void CallNoCallback()
        {
            _noCallback?.Invoke();
            Hide();
        }
    }
}
