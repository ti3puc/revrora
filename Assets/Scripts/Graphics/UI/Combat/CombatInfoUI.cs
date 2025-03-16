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

        [Header("HP Bars")]
        [SerializeField] private HpBarUI _playerHpSlider;
        [SerializeField] private HpBarUI _partyHpSlider;
        [SerializeField] private HpBarUI _enemyHpSlider;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<ItemCombatUI> _combatItemsUI;

        private void Awake()
        {
            CombatSystem.OnPlayerWonCombat += ShowWinScreen;
            CombatSystem.OnPlayerLostCombat += ShowLoseScreen;
            BaseCharacter.OnDamageReceived += UpdateHealthBar;
            TurnCombatManager.OnGivingItemsAfterCombat += SetupItemsToGive;
            TurnCombatManager.OnTurnManagerInitialized += SetupCharacterNames;

            foreach (var button in _exitButtons)
                button.onClick.AddListener(ExitBattle);

            _combatItemsUI = GetComponentsInChildren<ItemCombatUI>(true).ToList();
        }

        private void OnDestroy()
        {
            CombatSystem.OnPlayerWonCombat -= ShowWinScreen;
            CombatSystem.OnPlayerLostCombat -= ShowLoseScreen;
            BaseCharacter.OnDamageReceived -= UpdateHealthBar;
            TurnCombatManager.OnGivingItemsAfterCombat -= SetupItemsToGive;
            TurnCombatManager.OnTurnManagerInitialized -= SetupCharacterNames;

            foreach (var button in _exitButtons)
                button.onClick.RemoveListener(ExitBattle);
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

        private void SetupCharacterNames()
        {
            foreach (var character in TurnCombatManager.Instance.TurnCharacters)
            {
                if (character.CharacterTeam == CharacterTeam.Enemy)
                    _enemyHpSlider.PopulateCharacterInfo(character);
                else if (character.CharacterDefinition.IsPlayer)
                    _playerHpSlider.PopulateCharacterInfo(character);
                else
                    _partyHpSlider.PopulateCharacterInfo(character);
            }
        }

        private void UpdateHealthBar(BaseCharacter character)
        {
            if (character.CharacterTeam == CharacterTeam.Enemy)
                _enemyHpSlider.UpdateHealthBar(character);
            else if (character.CharacterDefinition.IsPlayer)
                _playerHpSlider.UpdateHealthBar(character);
            else
                _partyHpSlider.UpdateHealthBar(character);
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
