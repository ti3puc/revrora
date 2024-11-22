using System.Collections;
using System.Collections.Generic;
using Character.StateMachine.States;
using Managers;
using NaughtyAttributes;
using Persistence;
using UI.Menu;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private List<MainMenuPanelUI> _panels = new();
    [SerializeField] private Button _saveButton;

    [Header("Debug")]
    [SerializeField, ReadOnly] private MainMenuPanelUI _currentPanel;

    private void Awake()
    {
        foreach (var panel in _panels)
        {
            panel.SubscribeToEvents();
            panel.OnButtonClicked += UpdateCurrentPanel;
        }

        GameManager.OnPauseStateChanged += ShowSaveButton;
        _saveButton.onClick.AddListener(SaveGame);

        UpdateCurrentPanel(null);
    }

    private void OnDestroy()
    {
        foreach (var panel in _panels)
        {
            panel.UnsubscribeToEvents();
            panel.OnButtonClicked -= UpdateCurrentPanel;
        }

        GameManager.OnPauseStateChanged -= ShowSaveButton;
        _saveButton.onClick.RemoveListener(SaveGame);
    }

    private void UpdateCurrentPanel(MainMenuPanelUI panel)
    {
        _saveButton.interactable = true;

        if (panel == null)
            _currentPanel = _panels[0];
        else
            _currentPanel = panel;

        // update panel visual
        for (int i = 0; i < _panels.Count; i++)
        {
            if (_panels[i].DisableButtonsIfActive)
                _panels[i].ChangeButtonsState(_panels[i] != _currentPanel);

            _panels[i].GameObject.SetActive(_panels[i] == _currentPanel);
        }
    }

    private void ShowSaveButton(bool isPaused)
    {
        _saveButton.interactable = true;
        UpdateCurrentPanel(_panels[0]);
    }

    private void SaveGame()
    {
        SaveSystem.Instance.SaveGame();
        _saveButton.interactable = false;
    }
}
