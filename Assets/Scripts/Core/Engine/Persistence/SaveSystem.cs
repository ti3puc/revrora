using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Managers.Scenes;
using Managers.Settings;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Persistence
{
    public class SaveSystem : Singleton<SaveSystem>
    {
        public static event Action OnSaveLoaded;
        public event Action OnAutoSave;

        [Header("Settings")]
        [SerializeField] private int _maxSlots = 4;
        [SerializeField] private float _autoSaveInterval = 300f;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<string> _saveSlots;
        [SerializeField, ReadOnly] private float _autoSaveTimer = 0f;
        [SerializeField, ReadOnly] private GameData _currentGameData;

        private IDataService _dataService;

        public int MaxSlots => _maxSlots;
        public List<string> SaveSlots => _saveSlots = ListSaveSlots();
        public GameData GameData
        {
            get => _currentGameData;
            set => _currentGameData = value;
        }

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
            GameData = new GameData();

            if (SettingsManager.Instance.Autosave == 0)
            {
                Application.quitting += SaveGame;
                ScenesManager.OnAnySceneLoading += SaveGame;
            }
        }

        private void OnDestroy()
        {
            if (SettingsManager.Instance.Autosave == 0)
            {
                Application.quitting -= SaveGame;
                ScenesManager.OnAnySceneLoading -= SaveGame;
            }
        }

        private void LateUpdate()
        {
            if (_currentGameData == null) return;
            if (_autoSaveInterval <= 0) return;
            if (SettingsManager.Instance.Autosave == 1) return;

            _autoSaveTimer += Time.deltaTime;
            if (_autoSaveTimer >= _autoSaveInterval)
            {
                OnAutoSave?.Invoke();
                SaveGame();

                _autoSaveTimer = 0f;
            }
        }

        #endregion

        #region Public Methods

        [Button]
        public GameData CreateAvailableSaveSlot()
        {
            for (int i = 0; i < _maxSlots; i++)
            {
                if (!SaveSlots.Contains(GetSlotName(i)))
                    return CreateSaveSlot(i);
            }

            return null;
        }

        public GameData CreateSaveSlot(int slotIndex)
        {
            _currentGameData = new GameData
            {
                IndexId = slotIndex,
                CurrentScene = "Initial City",
                StartDate = DateTime.Now.ToString("o") // ISO 8601 format
            };

            return _currentGameData;
        }

        [Button]
        public void SaveGame()
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            if (_currentGameData == null)
            {
                Debug.LogError("Game Data is not set.");
                return;
            }

            _currentGameData.CurrentScene = SceneManager.GetActiveScene().name;
            if (_currentGameData.CurrentScene == "Main Menu" || _currentGameData.CurrentScene == "Combat")
                _currentGameData.CurrentScene = "Initial City";

            _currentGameData.LastPlayedDate = DateTime.Now.ToString("o");
            _dataService.Save(_currentGameData);
        }

        public void LoadSaveSlot(int slotIndex)
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            _currentGameData = GetSaveSlotData(slotIndex);

            if (string.IsNullOrWhiteSpace(_currentGameData.CurrentScene))
                _currentGameData.CurrentScene = "Initial City";

            OnSaveLoaded?.Invoke();
            ScenesManager.LoadScene(_currentGameData.CurrentScene);
        }

        public GameData GetSaveSlotData(int slotIndex)
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return null;
            }

            return _dataService.Load(GetSlotName(slotIndex));
        }

        public void DeleteSaveSlot(int slotIndex)
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            _dataService.Delete(GetSlotName(slotIndex));
        }

        [Button]
        public void DeleteAll()
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            PlayerPrefs.DeleteAll();
            _dataService.DeleteAll();
        }

        #endregion

        #region Private Methods

        private string GetSlotName(int slotIndex) => $"Slot {slotIndex}";

        private List<string> ListSaveSlots()
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return null;
            }

            return _dataService.ListSaves();
        }

        #endregion
    }
}