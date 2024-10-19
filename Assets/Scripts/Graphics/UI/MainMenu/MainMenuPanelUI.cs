using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    [Serializable]
    public class MainMenuPanelUI
    {
        public delegate void PanelEvent(MainMenuPanelUI mainMenuPanelUI);
        public event PanelEvent OnButtonClicked;

        public GameObject GameObject;
        public bool DisableButtonsIfActive;
        public Button[] ButtonsThatActivate;

        public void SubscribeToEvents()
        {
            foreach (var button in ButtonsThatActivate)
                button.onClick.AddListener(RaiseButtonClick);
        }

        public void UnsubscribeToEvents()
        {
            foreach (var button in ButtonsThatActivate)
                button.onClick.RemoveListener(RaiseButtonClick);
        }

        public void ChangeButtonsState(bool shouldEnable)
        {
            foreach (var button in ButtonsThatActivate)
                button.interactable = shouldEnable;
        }

        private void RaiseButtonClick()
        {
            OnButtonClicked?.Invoke(this);
            if (DisableButtonsIfActive)
                ChangeButtonsState(false);
        }
    }
}