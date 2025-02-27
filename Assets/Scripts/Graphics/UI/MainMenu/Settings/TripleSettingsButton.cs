using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Menu.Settings
{
    public class TripleSettingsButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _firstButton;
        [SerializeField] private Button _secondButton;
        [SerializeField] private Button _thirdButton;

        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onFirstButtonSelected;
        [SerializeField] private UnityEvent<int> _onSecondButtonSelected;
        [SerializeField] private UnityEvent<int> _onThirdButtonSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int _currentSelectedButton;

        private void Awake()
        {
            _firstButton.onClick.AddListener(SelectFirstButton);
            _secondButton.onClick.AddListener(SelectSecondButton);
            _thirdButton.onClick.AddListener(SelectThirdButton);

            UpdateSelectedButton(_currentSelectedButton);
        }

        private void OnDestroy()
        {
            _firstButton.onClick.RemoveListener(SelectFirstButton);
            _secondButton.onClick.RemoveListener(SelectSecondButton);
            _thirdButton.onClick.RemoveListener(SelectThirdButton);
        }

        public void UpdateSelectedButton(int index)
        {
            if (index < 0 || index > 2) return;

            _currentSelectedButton = index;
            Button selectedButton = index switch
            {
                0 => _firstButton,
                1 => _secondButton,
                2 => _thirdButton,
                _ => _firstButton
            };

            _firstButton.interactable = selectedButton != _firstButton;
            _secondButton.interactable = selectedButton != _secondButton;
            _thirdButton.interactable = selectedButton != _thirdButton;
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

        private void SelectThirdButton()
        {
            UpdateSelectedButton(2);
            _onThirdButtonSelected?.Invoke(2);
        }
    }
}
