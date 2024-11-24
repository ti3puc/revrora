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
        private void Awake()
        {
            CombatSystem.OnPlayerWonCombat += ShowWinScreen;
            CombatSystem.OnPlayerLostCombat += ShowLoseScreen;

            foreach (var button in _exitButtons)
                button.onClick.AddListener(ExitBattle);
            
            TurnInputManager.OnChangedInputCharacter += PopulateCharacterInfo;
        }

        private void OnDestroy()
        {
            CombatSystem.OnPlayerWonCombat -= ShowWinScreen;
            CombatSystem.OnPlayerLostCombat -= ShowLoseScreen;

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
            // TODO: logic to go back to last scene
            ScenesManager.LoadFirstScene();
        }
        
        private void PopulateCharacterInfo(BaseCharacter character)
        {
            _characterText.text = character.Name;
        }

        //TODO rodar ao perder vida
        private void UpdateHealthBar(BaseCharacter character)
        {
            if (character.CharacterTeam == CharacterTeam.Ally)
            {
                _playerHpSlider.value =
                    (float)Math.Round((float)(character.CharacterStats.HP / character.CharacterStats.MaxHP), 2);
            }
            else
            {
                _enemyHpSlider.value =
                    (float)Math.Round((float)(character.CharacterStats.HP / character.CharacterStats.MaxHP), 2);
            }
        }
    }
}
