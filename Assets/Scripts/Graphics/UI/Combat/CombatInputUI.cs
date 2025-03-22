using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Character.Class;
using Combat;
using Managers.Combat;
using Managers.Scenes;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        [Header("Name")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private string _namePrefix = "Waiting action from: ";

        [Header("Actions")]
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _backMovesButton;
        [SerializeField] private Button _runButton;

        [Header("Moves")]
        [SerializeField] private Vector3 _mouseOffset;
        [SerializeField] private RectTransform _moveInfoPanel;
        [SerializeField] private TMP_Text _moveInfo;
        [SerializeField] private Button _moveButton1;
        [SerializeField] private Button _moveButton2;
        [SerializeField] private Button _moveButton3;

        [Header("Moves: Color")]
        [SerializeField] private Color _normalColor = Color.magenta;
        [SerializeField] private Color _fireColor = Color.red;
        [SerializeField] private Color _waterColor = Color.blue;
        [SerializeField] private Color _earthColor = Color.green;
        [SerializeField] private Color _airColor = Color.grey;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isShowingMovesPanel;
        [SerializeField, ReadOnly] private BaseCharacter _currentCharacter;
        [SerializeField, ReadOnly] private bool _hasSetMoveInfo;

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

        private void LateUpdate()
        {
            _moveInfoPanel.gameObject.SetActive(_isShowingMovesPanel && _hasSetMoveInfo);

            if (_hasSetMoveInfo)
                _moveInfoPanel.position = Input.mousePosition + _mouseOffset;
        }

        public void SetMoveInfo(BaseEventData eventData)
        {
            if (_isShowingMovesPanel == false) return;
            if (_currentCharacter == null) return;

            PointerEventData pointerEventData = (PointerEventData)eventData;
            var moveIndex = pointerEventData.pointerEnter.transform.GetSiblingIndex();

            if (moveIndex >= _currentCharacter.CharacterMoves.Count) return;
            var move = _currentCharacter.CharacterMoves[moveIndex];

            var category = char.ToUpper(move.Category.ToString()[0]) + move.Category.ToString().Substring(1).ToLower();
            _moveInfo.text = $"Power: {move.Power}\nAccuracy: {move.Accuracy}\n\n{category} move: {move.MoveDescription}";

            _hasSetMoveInfo = true;
        }

        public void RemoveMoveInfo(BaseEventData eventData)
        {
            _hasSetMoveInfo = false;
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

            _hasSetMoveInfo = false;
            _isShowingMovesPanel = false;
        }

        private void ShowMoves()
        {
            _actionsPanel.gameObject.SetActive(false);
            _movesPanel.gameObject.SetActive(true);

            _hasSetMoveInfo = false;
            _isShowingMovesPanel = true;
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
            _currentCharacter = character;
            _nameText.text = _namePrefix + character.Name;

            // populate moves
            var moveButtons = new Button[] { _moveButton1, _moveButton2, _moveButton3 };
            for (int i = 0; i < moveButtons.Length; i++)
            {
                var moveNameText = moveButtons[i].transform.GetChild(0).GetComponent<TMP_Text>();
                var moveBox = moveButtons[i].transform.GetChild(1).GetComponent<Image>();
                var moveBoxText = moveBox.GetComponentInChildren<TMP_Text>();
                var move = character.CharacterMoves.Count > i ? character.CharacterMoves[i] : null;

                if (move != null)
                {
                    moveButtons[i].interactable = true;
                    moveNameText.text = $"{move.MoveName}";

                    moveBox.gameObject.SetActive(true);
                    moveBox.color = move.Type switch
                    {
                        CharacterTypes.NORMAL => _normalColor,
                        CharacterTypes.FIRE => _fireColor,
                        CharacterTypes.WATER => _waterColor,
                        CharacterTypes.EARTH => _earthColor,
                        CharacterTypes.AIR => _airColor,
                        _ => _normalColor,
                    };

                    var type = char.ToUpper(move.Type.ToString()[0]) + move.Type.ToString().Substring(1).ToLower();
                    moveBoxText.text = type;
                }
                else
                {
                    moveButtons[i].interactable = false;
                    moveNameText.text = "No move";
                    moveBox.gameObject.SetActive(false);
                }
            }
        }
    }
}
