using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Menu
{
    public class MainMenuSettingsUI : MonoBehaviour
    {
        [SerializeField] private List<MainMenuPanelUI> panels = new();

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI currentPanel;

        private void Awake()
        {
            foreach (var panel in panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            UpdateCurrentPanel(null);
        }

        private void OnDestroy()
        {
            foreach (var panel in panels)
            {
                panel.UnsubscribeToEvents();
                panel.OnButtonClicked -= UpdateCurrentPanel;
            }
        }

        private void UpdateCurrentPanel(MainMenuPanelUI panel)
        {
            if (panel == null)
                currentPanel = panels[0];
            else
                currentPanel = panel;

            // update panel visual
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].DisableButtonsIfActive)
                    panels[i].ChangeButtonsState(panels[i] != currentPanel);

                panels[i].GameObject.SetActive(panels[i] == currentPanel);
            }
        }
    }
}