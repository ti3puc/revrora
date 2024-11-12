using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers.Scenes;
using NaughtyAttributes;
using Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<MainMenuPanelUI> _panels = new();

        [Header("Saves")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI _currentPanel;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            foreach (var panel in _panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            _continueButton.onClick.AddListener(ContinueGame);

            UpdateCurrentPanel(null);
        }

        private void OnDestroy()
        {
            foreach (var panel in _panels)
            {
                panel.UnsubscribeToEvents();
                panel.OnButtonClicked -= UpdateCurrentPanel;
            }

            _continueButton.onClick.RemoveListener(ContinueGame);
        }

        private void OnEnable()
        {
            bool hasAnySave = SaveSystem.Instance.SaveSlots.Count > 0;
            _continueButton.gameObject.SetActive(hasAnySave);

            bool hasAvailableSaveSlot = SaveSystem.Instance.SaveSlots.Count < SaveSystem.Instance.MaxSlots;
            if (hasAvailableSaveSlot)
                _newGameButton.onClick.AddListener(StartNewGame);
            else
                _newGameButton.onClick.AddListener(ShowLoadPanel);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(StartNewGame);
            _newGameButton.onClick.RemoveListener(ShowLoadPanel);
        }

        #endregion

        #region Private Methods

        private void UpdateCurrentPanel(MainMenuPanelUI panel)
        {
            _currentPanel = panel;

            // update panel visual
            for (int i = 0; i < _panels.Count; i++)
            {
                if (_currentPanel == null)
                    _panels[i].GameObject.SetActive(i == 0);
                else
                    _panels[i].GameObject.SetActive(_panels[i] == _currentPanel);
            }
        }

        private void ShowLoadPanel()
        {
            var loadPanel = _panels.Find(panel => panel.GameObject.name.Contains("Load"));
            if (loadPanel == null)
            {
                Debug.LogError("Could not find Load Panel", this);
                return;
            }

            UpdateCurrentPanel(loadPanel);
        }

        #endregion

        #region Public Methods

        public void StartNewGame()
        {
            var newSave = SaveSystem.Instance.CreateAvailableSaveSlot();
            SaveSystem.Instance.SaveGame();
            ScenesManager.LoadScene(newSave.CurrentScene);
        }

        public void ContinueGame()
        {
            if (SaveSystem.Instance.SaveSlots.Count <= 0)
                return;

            var mostRecentSave = SaveSystem.Instance.SaveSlots
                .Where(slot => slot != null)
                .OrderByDescending(slot => DateTime.Parse(SaveSystem.Instance.GetSaveSlotData(int.Parse(slot.Replace("Slot", ""))).LastPlayedDate))
                .FirstOrDefault();

            if (mostRecentSave != null)
            {
                var saveSlotData = SaveSystem.Instance.GetSaveSlotData(int.Parse(mostRecentSave.Replace("Slot", "")));
                SaveSystem.Instance.LoadSaveSlot(saveSlotData.IndexId);
            }
        }

        #region Methods for UnityEvent
        // These functions are used inside Unity editor, on the Button
        // component. Thats why vscode maybe wont find the reference
        // where they're being used.

        public void QuitGame()
        {
            Application.Quit();
        }
        #endregion

        #endregion
    }
}
