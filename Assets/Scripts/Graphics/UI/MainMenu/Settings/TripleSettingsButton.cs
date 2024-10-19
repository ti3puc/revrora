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
        [SerializeField] private Button firstButton;
        [SerializeField] private Button secondButton;
        [SerializeField] private Button thirdButton;

        [Header("Events")]
        [SerializeField] private UnityEvent onFirstButtonSelected;
        [SerializeField] private UnityEvent onSecondButtonSelected;
        [SerializeField] private UnityEvent onThirdButtonSelected;

        [Header("Debug")]
        [SerializeField, ReadOnly] private Button currentSelectedButton;

        private void Awake()
        {
            firstButton.onClick.AddListener(SelectFirstButton);
            secondButton.onClick.AddListener(SelectSecondButton);
            thirdButton.onClick.AddListener(SelectThirdButton);

            // TODO: get settings from save
            UpdateSelectedButton(firstButton);
        }

        private void OnDestroy()
        {
            firstButton.onClick.RemoveListener(SelectFirstButton);
            secondButton.onClick.RemoveListener(SelectSecondButton);
            thirdButton.onClick.RemoveListener(SelectThirdButton);
        }

        private void UpdateSelectedButton(Button selectedButton)
        {
            firstButton.interactable = selectedButton != firstButton;
            secondButton.interactable = selectedButton != secondButton;
            thirdButton.interactable = selectedButton != thirdButton;

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

        private void SelectThirdButton()
        {
            UpdateSelectedButton(thirdButton);
            onThirdButtonSelected?.Invoke();
        }
    }
}
