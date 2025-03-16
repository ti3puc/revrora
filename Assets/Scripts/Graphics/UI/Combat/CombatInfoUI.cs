using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Managers.Scenes;
using Managers.Combat;
using Character.Base;
using Character.Class;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using NaughtyAttributes;

namespace UI.Combat
{
    public class CombatInfoUI : MonoBehaviour
    {
        [Header("Win/Lose Screens")]
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private Button[] _exitButtons;

        [Header("Text")]
        [SerializeField] private TMP_Text _characterText;

        [Header("HP Bar")]
        [SerializeField] private Slider _playerHpSlider;
        [SerializeField] private Slider _enemyHpSlider;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<ItemCombatUI> _combatItemsUI;

        private void Awake()
        {
            CombatSystem.OnPlayerWonCombat += ShowWinScreen;
            CombatSystem.OnPlayerLostCombat += ShowLoseScreen;
            BaseCharacter.OnDamageReceived += UpdateHealthBar;
            TurnCombatManager.OnGivingItemsAfterCombat += SetupItemsToGive;

            foreach (var button in _exitButtons)
                button.onClick.AddListener(ExitBattle);

            TurnInputManager.OnChangedInputCharacter += PopulateCharacterInfo;

            _combatItemsUI = GetComponentsInChildren<ItemCombatUI>(true).ToList();
        }

        private void OnDestroy()
        {
            CombatSystem.OnPlayerWonCombat -= ShowWinScreen;
            CombatSystem.OnPlayerLostCombat -= ShowLoseScreen;
            BaseCharacter.OnDamageReceived -= UpdateHealthBar;
            TurnCombatManager.OnGivingItemsAfterCombat -= SetupItemsToGive;

            foreach (var button in _exitButtons)
                button.onClick.RemoveListener(ExitBattle);

            TurnInputManager.OnChangedInputCharacter -= PopulateCharacterInfo;
        }

        private void ShowWinScreen()
        {
            _winScreen.SetActive(true);
            _loseScreen.SetActive(false);
        }

        private void ShowLoseScreen()
        {
            _winScreen.SetActive(false);
            _loseScreen.SetActive(true);
        }

        private void ExitBattle()
        {
            ScenesManager.LoadLastScene();
        }

        private void PopulateCharacterInfo(BaseCharacter character)
        {
            _characterText.text = character.Name;
        }

        private void UpdateHealthBar(BaseCharacter character)
        {
            float value = (float)character.CharacterStats.HP / character.CharacterStats.MaxHP;
            double roundedValue = Math.Round(value, 2);
            if (character.CharacterTeam == CharacterTeam.Ally)
            {
                _playerHpSlider.value = (float)roundedValue;
            }
            else
            {
                _enemyHpSlider.value = (float)roundedValue;
            }
        }

        private void SetupItemsToGive(List<CombatDropItem> itemsToGive)
        {
            if (_combatItemsUI == null || _combatItemsUI.Count <= 0)
            {
                GameLog.Error(this, "List of UI combat items references is empty.");
                return;
            }

            foreach (var itemUI in _combatItemsUI)
            {
                var item = itemsToGive.FirstOrDefault(i => i.ItemReference == itemUI.ItemReference);
                itemUI.SetupItemToGive(item);
            }
        }
    }
}
