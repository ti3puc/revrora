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
using Core.Engine.Combat.CombatActions;

namespace UI.Combat
{
    public class CombatInfoUI : MonoBehaviour
    {
        public static event Action OnShouldShowActions;

        [Header("Win/Lose Screens")]
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private Button[] _exitButtons;

        [Header("HP Bars")]
        [SerializeField] private HpBarUI _playerHpSlider;
        [SerializeField] private HpBarUI _partyHpSlider;
        [SerializeField] private HpBarUI _enemyHpSlider;

        [Header("Moves History")]
        [SerializeField] private ScrollRect _movesHistoryPanel;
        [SerializeField] private Transform _movesHistoryContent;
        [SerializeField] private GameObject _movesHistoryPrefab;
        [SerializeField] private Scrollbar _movesHistoryScrollbar;
        [SerializeField] private Button _backToActionButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<ItemCombatUI> _combatItemsUI;

        private void Awake()
        {
            CombatSystem.OnPlayerWonCombat += ShowWinScreen;
            CombatSystem.OnPlayerLostCombat += ShowLoseScreen;

            BaseCharacter.OnDamageReceived += UpdateHealthBar;

            TurnCombatManager.OnGivingItemsAfterCombat += SetupItemsToGive;
            TurnCombatManager.OnTurnManagerInitialized += SetupCharacterNames;

            CanvasManager.OnTogglePauseCanvas += ToggleUI;

            CombatInputUI.OnShouldShowCombatLogs += ShowMovesHistory;

            CombatSystem.OnStartedProcessingCombat += ShowMovesHistory;
            CombatSystem.OnFinishedProcessingCombat += ShowActions;

            CombatActionMove.OnMoveHistory += AddMoveToHistory;
            BaseCharacter.OnCharacterDied += AddDeadToHistory;

            foreach (var button in _exitButtons)
                button.onClick.AddListener(ExitBattle);

            _combatItemsUI = GetComponentsInChildren<ItemCombatUI>(true).ToList();

            _backToActionButton.onClick.AddListener(ShowActions);
        }

        private void OnDestroy()
        {
            CombatSystem.OnPlayerWonCombat -= ShowWinScreen;
            CombatSystem.OnPlayerLostCombat -= ShowLoseScreen;

            BaseCharacter.OnDamageReceived -= UpdateHealthBar;

            TurnCombatManager.OnGivingItemsAfterCombat -= SetupItemsToGive;
            TurnCombatManager.OnTurnManagerInitialized -= SetupCharacterNames;

            CanvasManager.OnTogglePauseCanvas -= ToggleUI;

            CombatInputUI.OnShouldShowCombatLogs -= ShowMovesHistory;

            CombatSystem.OnStartedProcessingCombat -= ShowMovesHistory;
            CombatSystem.OnFinishedProcessingCombat -= ShowActions;

            CombatActionMove.OnMoveHistory -= AddMoveToHistory;
            BaseCharacter.OnCharacterDied -= AddDeadToHistory;

            foreach (var button in _exitButtons)
                button.onClick.RemoveListener(ExitBattle);

            _backToActionButton.onClick.RemoveListener(ShowActions);
        }

        private void AddDeadToHistory(BaseCharacter character)
        {
            var history = Instantiate(_movesHistoryPrefab, _movesHistoryContent.transform);
            var historyText = history.GetComponent<TMP_Text>();
            historyText.text = $"{character.Name} died.";

            UpdateHistoryScrollbar();
        }

        private void AddMoveToHistory(string historyStr)
        {
            var history = Instantiate(_movesHistoryPrefab, _movesHistoryContent.transform);
            var historyText = history.GetComponent<TMP_Text>();
            historyText.text = historyStr;

            UpdateHistoryScrollbar();
        }

        private void UpdateHistoryScrollbar()
        {
            _movesHistoryScrollbar.value = 1;
            _movesHistoryPanel.verticalNormalizedPosition = 0;

            StartCoroutine(DelayUpdateHistoryScrollbar());
        }

        private IEnumerator DelayUpdateHistoryScrollbar()
        {
            yield return new WaitForSeconds(0.1f);
            _movesHistoryScrollbar.value = 1;
            _movesHistoryPanel.verticalNormalizedPosition = 0;
        }

        private void ShowActions()
        {
            _movesHistoryPanel.gameObject.SetActive(false);
            _backToActionButton.gameObject.SetActive(false);
            OnShouldShowActions?.Invoke();
        }

        private void ShowMovesHistory()
        {
            _movesHistoryPanel.gameObject.SetActive(true);
            _backToActionButton.gameObject.SetActive(true);
        }

        private void ToggleUI(bool state)
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = state;
            ShowActions();
        }

        private void ShowWinScreen()
        {
            _winScreen.SetActive(true);
            _loseScreen.SetActive(false);

            _movesHistoryPanel.gameObject.SetActive(false);
            _backToActionButton.gameObject.SetActive(false);
        }

        private void ShowLoseScreen()
        {
            _winScreen.SetActive(false);
            _loseScreen.SetActive(true);

            _movesHistoryPanel.gameObject.SetActive(false);
            _backToActionButton.gameObject.SetActive(false);
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
