using System.Collections;
using System.Collections.Generic;
using Persistence;
using TMPro;
using UnityEngine;

namespace UI.Menu.Load
{
    public class SaveSlotUI : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private GameObject _emptyState;
        [SerializeField] private GameObject _withDataState;

        [Header("Slot Info")]
        [SerializeField] private TMP_Text _slotName;

        public void UpdateSlot(GameData slotGameData)
        {
            if (slotGameData == null)
            {
                _emptyState.SetActive(true);
                _withDataState.SetActive(false);
                return;
            }

            _emptyState.SetActive(false);
            _withDataState.SetActive(true);
            _slotName.text = $"Slot {slotGameData.Id + 1}";
        }
    }
}
