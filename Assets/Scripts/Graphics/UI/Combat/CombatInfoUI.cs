using System.Collections;
using System.Collections.Generic;
using Combat;
using Managers.Scenes;
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

        private void Awake()
        {
            CombatSystem.OnPlayerWonCombat += ShowWinScreen;
            CombatSystem.OnPlayerLostCombat += ShowLoseScreen;

            foreach (var button in _exitButtons)
                button.onClick.AddListener(ExitBattle);
        }

        private void OnDestroy()
        {
            CombatSystem.OnPlayerWonCombat -= ShowWinScreen;
            CombatSystem.OnPlayerLostCombat -= ShowLoseScreen;

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
            // TODO: logic to go back to last scene
            ScenesManager.LoadFirstScene();
        }
    }
}
