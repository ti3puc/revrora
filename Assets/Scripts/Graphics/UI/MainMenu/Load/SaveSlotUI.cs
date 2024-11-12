using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Load
{
    public class SaveSlotUI : MonoBehaviour
    {
        public delegate void SlotSelectedEvent(Action confirmCallback, Action cancelCallback);
        public static event SlotSelectedEvent OnSlotNewSaveSelected;
        public static event SlotSelectedEvent OnSlotLoadSaveSelected;
        public static event SlotSelectedEvent OnSlotDeleteSaveSelected;

        [Header("States")]
        [SerializeField] private GameObject _emptyState;
        [SerializeField] private GameObject _withDataState;

        [Header("Slot Info")]
        [SerializeField] private TMP_Text _slotName;
        [SerializeField] private TMP_Text _slotPlayTime;

        [Header("Slot Buttons")]
        [SerializeField] private Button _emptyButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _deleteButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int _slotIndex = -1;
        [SerializeField, ReadOnly] private GameData _currentSlotGameData;

        private void Awake()
        {
            _emptyButton.onClick.AddListener(NewSlot);
            _loadButton.onClick.AddListener(LoadSlot);
            _deleteButton.onClick.AddListener(DeleteSlot);
        }

        private void OnDestroy()
        {
            _emptyButton.onClick.RemoveListener(NewSlot);
            _loadButton.onClick.RemoveListener(LoadSlot);
            _deleteButton.onClick.RemoveListener(DeleteSlot);
        }

        public void UpdateSlot(int slotIndex)
        {
            _slotIndex = slotIndex;
            UpdateSlot(SaveSystem.Instance.GetSaveSlotData(slotIndex));
        }

        public void UpdateSlot(GameData slotGameData)
        {
            if (slotGameData == null)
            {
                _emptyState.SetActive(true);
                _withDataState.SetActive(false);
                return;
            }

            _currentSlotGameData = slotGameData;

            _emptyState.SetActive(false);
            _withDataState.SetActive(true);

            _slotName.text = $"Slot {slotGameData.IndexId + 1}";

            if (string.IsNullOrEmpty(slotGameData.LastPlayedDate) || string.IsNullOrEmpty(slotGameData.StartDate))
            {
                _slotPlayTime.text = "00:00:00";
                return;
            }

            var lastPlayedDate = DateTime.Parse(slotGameData.LastPlayedDate);
            var startDate = DateTime.Parse(slotGameData.StartDate);
            var playTime = lastPlayedDate - startDate;
            _slotPlayTime.text = $"{playTime.ToString(@"hh\:mm\:ss")}";
        }

        private void NewSlot()
        {
            OnSlotNewSaveSelected?.Invoke(() =>
            {
                _currentSlotGameData = SaveSystem.Instance.CreateSaveSlot(_slotIndex);
                SaveSystem.Instance.SaveGame();

                SaveSystem.Instance.LoadSaveSlot(_currentSlotGameData.IndexId);
            }, null);
        }

        private void LoadSlot()
        {
            OnSlotLoadSaveSelected?.Invoke(() =>
            {
                SaveSystem.Instance.LoadSaveSlot(_currentSlotGameData.IndexId);
            }, null);
        }

        private void DeleteSlot()
        {
            OnSlotDeleteSaveSelected?.Invoke(() =>
            {
                SaveSystem.Instance.DeleteSaveSlot(_currentSlotGameData.IndexId);
                _currentSlotGameData = null;
                UpdateSlot(_currentSlotGameData);
            }, null);
        }
    }
}
