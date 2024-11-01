using System.Collections;
using System.Collections.Generic;
using Character.Base;
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
            if (_hasReceivedPlayerInput)
            {
                
            }
        }

        private void ProcessCombat()
        {
            while (!TurnCombatManager.Instance.IsTurnEnd)
            {
                _currentCharacter = TurnCombatManager.Instance.GetCurrentCharacter();

                switch (_currentCharacter.CombatAction.ActionType)
                {
                    case CombatActionType.Attack:
                        _currentCharacter.CombatAction.Move.DoMove();
                        break;
                    case CombatActionType.Defense:
                        break;
                    case CombatActionType.Heal:
                        break;
                    default:
                        break;
                }

                TurnCombatManager.Instance.SetNextCharacter();
            }
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