using System;
using System.Collections;
using System.Collections.Generic;
using Persistence;
using UI.Menu.Load;
using UnityEngine;

namespace UI.Menu
{
    public class MainMenuLoadUI : MonoBehaviour
    {
        [SerializeField] private List<SaveSlotUI> _saveSlots;

        private void OnEnable()
        {
            for (int i = 0; i < _saveSlots.Count; i++)
            {
                var gameData = SaveSystem.Instance.GetSaveSlotData(i);
                _saveSlots[i].UpdateSlot(gameData);
            }
        }
    }
}
