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
        [SerializeField] private Button firstButton;
        [SerializeField] private Button secondButton;

        [Header("Events")]
        [SerializeField] private UnityEvent onFirstButtonSelected;
        [SerializeField] private UnityEvent onSecondButtonSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Button currentSelectedButton;

        private void Awake()
        {
            firstButton.onClick.AddListener(SelectFirstButton);
            secondButton.onClick.AddListener(SelectSecondButton);

            // TODO: get settings from save
            UpdateSelectedButton(firstButton);
        }

        private void OnDestroy()
        {
            firstButton.onClick.RemoveListener(SelectFirstButton);
            secondButton.onClick.RemoveListener(SelectSecondButton);
        }

        private void UpdateSelectedButton(Button selectedButton)
        {
            firstButton.interactable = selectedButton != firstButton;
            secondButton.interactable = selectedButton != secondButton;

            currentSelectedButton = selectedButton;
        }

        private void SelectFirstButton()
        {
            UpdateSelectedButton(firstButton);
            onFirstButtonSelected?.Invoke();
        }

        private void SelectSecondButton()
        {
            UpdateSelectedButton(secondButton);
            onSecondButtonSelected?.Invoke();
        }
    }
}
