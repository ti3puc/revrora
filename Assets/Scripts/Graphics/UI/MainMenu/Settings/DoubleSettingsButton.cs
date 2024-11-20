using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Menu.Settings
{
    public class DoubleSettingsButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _firstButton;
        [SerializeField] private Button _secondButton;

        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onFirstButtonSelected;
        [SerializeField] private UnityEvent<int> _onSecondButtonSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int _currentSelectedButton;

        private void Awake()
        {
            _firstButton.onClick.AddListener(SelectFirstButton);
            _secondButton.onClick.AddListener(SelectSecondButton);

            UpdateSelectedButton(0);
        }

        private void OnDestroy()
        {
            _firstButton.onClick.RemoveListener(SelectFirstButton);
            _secondButton.onClick.RemoveListener(SelectSecondButton);
        }

        public void UpdateSelectedButton(int index)
        {
            if (index < 0 || index > 1) return;

            _currentSelectedButton = index;
            Button selectedButton = index switch
            {
                0 => _firstButton,
                1 => _secondButton,
                _ => _firstButton
            };

            _firstButton.interactable = selectedButton != _firstButton;
            _secondButton.interactable = selectedButton != _secondButton;
        }

        private void SelectFirstButton()
        {
            UpdateSelectedButton(0);
            _onFirstButtonSelected?.Invoke(0);
        }

        private void SelectSecondButton()
        {
            UpdateSelectedButton(1);
            _onSecondButtonSelected?.Invoke(1);
        }
    }
}
