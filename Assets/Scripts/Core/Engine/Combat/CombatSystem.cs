using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Base;
using Core.Engine.Combat.CombatActions;
using Managers.Combat;
using NaughtyAttributes;
using Player.Input;
using UI.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        public static event Action OnStartedProcessingCombat;
        public static event Action OnFinishedProcessingCombat;
        public static event Action OnCombatEnded;
        public static event Action OnPlayerWonCombat;
        public static event Action OnPlayerLostCombat;


        [Header("Debug")]
        [SerializeField, ReadOnly] private BaseCharacter _currentCharacter;
        [SerializeField, ReadOnly] private BaseCharacter _currentInputCharacter;
        [SerializeField, ReadOnly] private bool _isReceivingTurnInput = true;
        [SerializeField, ReadOnly] private bool _hasFinishedTurnInputs;
        [SerializeField, ReadOnly] private bool _isCombatEnded;
        [SerializeField, ReadOnly] private Dictionary<BaseCharacter, int> _turnMoveDict = new();

        private void Awake()
        {
            CombatInputUI.OnMoveCalled += ChooseMove;
            _isReceivingTurnInput = true;

            // TODO: start with pokemons from scene
            TurnCombatManager.Instance.DebugInitialize();
        }

        private void OnDestroy()
        {
            CombatInputUI.OnMoveCalled -= ChooseMove;
        }

        private void Update()
        {
            if (_isCombatEnded) return;

            if (TurnCombatManager.Instance.HasInitialized && _isReceivingTurnInput)
            {
                StartCoroutine(GetCombatInputs());
                _isReceivingTurnInput = false;
            }

            if (TurnCombatManager.Instance.HasInitialized && _hasFinishedTurnInputs)
            {
                StartCoroutine(ProcessCombatCoroutine());
                _hasFinishedTurnInputs = false;
            }
        }

        private IEnumerator GetCombatInputs()
        {
            while (!TurnInputManager.Instance.IsTurnInputEnd)
            {
                _currentInputCharacter = TurnInputManager.Instance.GetCurrentCharacter();

                if (_currentInputCharacter.CharacterStats.IsDead())
                {
                    TurnInputManager.Instance.SetNextCharacter();
                    continue;
                }

                if (_currentInputCharacter.CharacterMoves.Count == 0)
                {
                    GameLog.Warning(this, $"{_currentInputCharacter.Name} has no moves.");
                    TurnInputManager.Instance.SetNextCharacter();
                    continue;
                }

                if (_currentInputCharacter != null && !_currentInputCharacter.IsTeamPlayer)
                {
                    // TODO: logic to choose move for enemies
                    var randomIndex = Random.Range(0, _currentInputCharacter.CharacterMoves.Count);
                    AddMove(_currentInputCharacter, randomIndex);
                }

                yield return null;
            }

            _hasFinishedTurnInputs = true;
            TurnInputManager.Instance.SetNewTurn();
        }

        private IEnumerator ProcessCombatCoroutine()
        {
            OnStartedProcessingCombat?.Invoke();

            while (!TurnCombatManager.Instance.IsTurnEnd)
            {
                _currentCharacter = TurnCombatManager.Instance.GetCurrentCharacter();

                if (_currentCharacter.CharacterStats.IsDead())
                {
                    TurnCombatManager.Instance.SetNextCharacter();
                    continue;
                }

                if (_currentCharacter.CharacterMoves.Count == 0)
                {
                    GameLog.Warning(this, $"{_currentCharacter.Name} has no moves.");
                    TurnCombatManager.Instance.SetNextCharacter();
                    continue;
                }

                var move = _currentCharacter.CharacterMoves[_turnMoveDict[_currentCharacter]];
                new CombatActionMove().execute(_currentCharacter, move, TurnCombatManager.Instance.GetEnemies(_currentCharacter));

                TurnCombatManager.Instance.SetNextCharacter();
                yield return new WaitForSeconds(1f);
            }

            // if combat end there's no need to finish processing
            if (HasCombatEnded() == false)
            {
                TurnCombatManager.Instance.SetNewTurn();
                _turnMoveDict.Clear();

                _isReceivingTurnInput = true;
                OnFinishedProcessingCombat?.Invoke();
            }
        }

        private void ChooseMove(int moveIndex)
        {
            if (_currentInputCharacter != null)
                AddMove(_currentInputCharacter, moveIndex);
        }

        private void AddMove(BaseCharacter character, int moveIndex)
        {
            if (!_turnMoveDict.ContainsKey(character))
                _turnMoveDict.Add(character, moveIndex);

            TurnInputManager.Instance.SetNextCharacter();
        }

        private bool HasCombatEnded()
        {
            var turnCharacters = TurnCombatManager.Instance.TurnCharacters;
            var aliveCharacters = turnCharacters.Where(c => !c.CharacterStats.IsDead()).ToList();

            var teamPlayers = aliveCharacters.Where(c => c.IsTeamPlayer).ToList();
            var enemies = aliveCharacters.Where(c => !c.IsTeamPlayer).ToList();

            if (teamPlayers.Count == 0)
            {
                EndCombat(false);
                return true;
            }
            else if (enemies.Count == 0)
            {
                EndCombat(true);
                return true;
            }

            return false;
        }

        private void EndCombat(bool isWin)
        {
            if (isWin)
            {
                GameLog.Debug(this, "You win!");
                OnPlayerWonCombat?.Invoke();
            }
            else
            {
                GameLog.Debug(this, "You lose!");
                OnPlayerLostCombat?.Invoke();
            }

            OnCombatEnded?.Invoke();
            _isCombatEnded = true;
        }
    }
}