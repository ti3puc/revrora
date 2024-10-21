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
        [SerializeField] private UnityEvent _onFirstButtonSelected;
        [SerializeField] private UnityEvent _onSecondButtonSelected;
        [SerializeField] private UnityEvent _onThirdButtonSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Button _currentSelectedButton;

        private void Awake()
        {
            _firstButton.onClick.AddListener(SelectFirstButton);
            _secondButton.onClick.AddListener(SelectSecondButton);
            _thirdButton.onClick.AddListener(SelectThirdButton);

            // TODO: get settings from save
            UpdateSelectedButton(_firstButton);
        }

        private void OnDestroy()
        {
            _firstButton.onClick.RemoveListener(SelectFirstButton);
            _secondButton.onClick.RemoveListener(SelectSecondButton);
            _thirdButton.onClick.RemoveListener(SelectThirdButton);
        }

        private void UpdateSelectedButton(Button selectedButton)
        {
            _firstButton.interactable = selectedButton != _firstButton;
            _secondButton.interactable = selectedButton != _secondButton;
            _thirdButton.interactable = selectedButton != _thirdButton;

            _currentSelectedButton = selectedButton;
        }

        private void SelectFirstButton()
        {
            UpdateSelectedButton(_firstButton);
            _onFirstButtonSelected?.Invoke();
        }

        private void SelectSecondButton()
        {
            UpdateSelectedButton(_secondButton);
            _onSecondButtonSelected?.Invoke();
        }

        private void SelectThirdButton()
        {
            UpdateSelectedButton(_thirdButton);
            _onThirdButtonSelected?.Invoke();
        }
    }
}
