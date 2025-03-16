using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Combat;
using Managers.Combat;
using Managers.Scenes;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    public class CombatInputUI : MonoBehaviour
    {
        public delegate void MoveEvent(int moveIndex);
        public static event MoveEvent OnMoveCalled;

        [Header("Canvas")]
        [SerializeField] private Canvas _canvas;

        [Header("Panels")]
        [SerializeField] private GameObject _actionsPanel;
        [SerializeField] private GameObject _movesPanel;

        [Header("Panels")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private string _namePrefix = "Waiting action from: ";

        [Header("Action Buttons")]
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _backMovesButton;
        [SerializeField] private Button _runButton;

        [Header("Move Buttons")]
        [SerializeField] private Button _moveButton1;
        [SerializeField] private Button _moveButton2;
        [SerializeField] private Button _moveButton3;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isShowingMovesPanel;

        private void Awake()
        {
            _attackButton.onClick.AddListener(ShowMoves);
            _backMovesButton.onClick.AddListener(ShowActions);
            _runButton.onClick.AddListener(ExitBattle);

            // idk why, tried with button list but didnt work
            _moveButton1.onClick.AddListener(CallMove1);
            _moveButton2.onClick.AddListener(CallMove2);
            _moveButton3.onClick.AddListener(CallMove3);

            CombatSystem.OnStartedProcessingCombat += HideUI;
            CombatSystem.OnFinishedProcessingCombat += ShowUI;

            CombatSystem.OnCombatEnded += HideUI;

            ShowActions();
            
            TurnInputManager.OnChangedInputCharacter += OnChangedInputCharacter;
        }

        private void OnDestroy()
        {
            _attackButton.onClick.RemoveListener(ShowMoves);
            _backMovesButton.onClick.RemoveListener(ShowActions);
            _runButton.onClick.RemoveListener(ExitBattle);

            _moveButton1.onClick.RemoveListener(CallMove1);
            _moveButton2.onClick.RemoveListener(CallMove2);
            _moveButton3.onClick.RemoveListener(CallMove3);

            CombatSystem.OnStartedProcessingCombat -= HideUI;
            CombatSystem.OnFinishedProcessingCombat -= ShowUI;

            CombatSystem.OnCombatEnded -= HideUI;
            
            TurnInputManager.OnChangedInputCharacter -= OnChangedInputCharacter;
        }

        private void HideUI()
        {
            _canvas.enabled = false;
        }

        private void ShowUI()
        {
            _canvas.enabled = true;
        }

        private void ShowActions()
        {
            _actionsPanel.gameObject.SetActive(true);
            _movesPanel.gameObject.SetActive(false);
        }

        private void ShowMoves()
        {
            _actionsPanel.gameObject.SetActive(false);
            _movesPanel.gameObject.SetActive(true);
        }

        private void CallMove1() => CallMove(0);
        private void CallMove2() => CallMove(1);
        private void CallMove3() => CallMove(2);

        private void CallMove(int moveIndex)
        {
            OnMoveCalled?.Invoke(moveIndex);
            ShowActions();
        }

        private void ExitBattle()
        {
            ScenesManager.LoadLastScene();
        }
        
        private void OnChangedInputCharacter(BaseCharacter character)
        {
            _nameText.text = _namePrefix + character.Name;

            // populate moves
            var moveButtons = new Button[] { _moveButton1, _moveButton2, _moveButton3 };
            for (int i = 0; i < moveButtons.Length; i++)
            {
                var moveButtonText = moveButtons[i].GetComponentInChildren<TMP_Text>();
                var move = character.CharacterMoves.Count > i ? character.CharacterMoves[i] : null;

                if (move != null)
                {
                    moveButtons[i].interactable = true;
                    moveButtonText.text = move.MoveName;
                }
                else
                {
                    moveButtons[i].interactable = false;
                    moveButtonText.text = "No move";
                }
            }
        }
    }
}
