using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Persistence
{
    public class SaveSystem : Singleton<SaveSystem>
    {
        [Header("Settings")]
        [SerializeField] private int _maxSlots = 4;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<string> _saveSlots;

        private GameData _gameData;
        private IDataService _dataService;

        public GameData GameData => _gameData;
        public int MaxSlots => _maxSlots;
        public List<string> SaveSlots => _saveSlots = ListSaveSlots();

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
        }

        #endregion

        #region Public Methods

        [Button]
        public void CreateSaveSlot(int slotIndex = 1)
        {
            _gameData = new GameData
            {
                Id = slotIndex,
                CurrentScene = "Initial City",
                StartDate = DateTime.Now.ToString("o") // ISO 8601 format
            };
        }

        [Button]
        public void SaveGame()
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            if (_gameData == null)
            {
                Debug.LogError("Game Data is not set.");
                return;
            }

            _gameData.LastPlayedDate = DateTime.Now.ToString("o");
            _dataService.Save(_gameData);
        }

        public void LoadSaveSlot(int slotIndex = 1)
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            _gameData = _dataService.Load(GetSlotName(slotIndex));

            if (string.IsNullOrWhiteSpace(_gameData.CurrentScene))
                _gameData.CurrentScene = "Initial City";
        }

        public void DeleteSaveSlot(int slotIndex = 1)
        {
            if (_dataService == null)
            {
                Debug.LogError("Data service is not set.");
                return;
            }

            _dataService.Delete(GetSlotName(slotIndex));
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