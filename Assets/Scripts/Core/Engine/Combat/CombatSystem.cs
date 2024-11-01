using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Core.Engine.Combat.CombatActions;
using Managers.Combat;
using NaughtyAttributes;
using Player.Input;
using UnityEngine;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] private BaseCharacter _currentCharacter;
        [SerializeField, ReadOnly] private bool _hasReceivedPlayerInput;

        private void Awake()
        {
            PlayerInput.OnInteractionStarted += ReceivedInteraction;
            PlayerInput.OnInteractionCanceled += CanceledInteraction;
        }

        private void OnDestroy()
        {
            PlayerInput.OnInteractionStarted -= ReceivedInteraction;
            PlayerInput.OnInteractionCanceled -= CanceledInteraction;
        }

        private void Update()
        {
            if (TurnCombatManager.Instance.HasInitialized && _hasReceivedPlayerInput)
            {
                StartCoroutine(ProcessCombatCoroutine());
                _hasReceivedPlayerInput = false;
            }
        }

        private IEnumerator ProcessCombatCoroutine()
        {
            while (!TurnCombatManager.Instance.IsTurnEnd)
            {
                _currentCharacter = TurnCombatManager.Instance.GetCurrentCharacter();

                if (_currentCharacter.CharacterMoves.Count == 0)
                {
                    GameLog.Warning(this, $"{_currentCharacter.Name} has no moves.");
                    TurnCombatManager.Instance.SetNextCharacter();
                    continue;
                }
                
                var tackle = _currentCharacter.CharacterMoves[0];
                
                new CombatActionMove().execute(_currentCharacter, tackle, TurnCombatManager.Instance.GetEnemies(_currentCharacter));

                yield return new WaitForSeconds(1f);
                TurnCombatManager.Instance.SetNextCharacter();
            }

            TurnCombatManager.Instance.SetNewTurn();
        }

        private void ReceivedInteraction()
        {
            _hasReceivedPlayerInput = true;
        }

        private void CanceledInteraction()
        {
            _hasReceivedPlayerInput = false;
        }
    }
}