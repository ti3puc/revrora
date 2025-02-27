using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Character.StateMachine;
using Environment.Interaction;
using Infra.Handler;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Npc.Dialog
{
    public class NpcDialog : Interactable
    {
        [Header("Dialogue Scriptable")]
        [SerializeField] private Dialogue _dialogue;

        [Header("Character")]
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private CharacterStateMachine _characterStateMachine;
        [SerializeField] private bool _useNameReference;
        [SerializeField, ShowIf("_useNameReference")] private BaseCharacter _characterForName;

        [Header("Animation")]
        [SerializeField] private bool _useRotation = true;

        [Header("Debug")]
        [SerializeField, ReadOnly] private NavMeshAgent _navMeshAgent;
        [SerializeField, ReadOnly] private bool _isLookingAtInteractor;
        private ICharacterState _lastState;

        protected override void Awake()
        {
            base.Awake();
            if (_characterStateMachine != null)
                _navMeshAgent = _characterStateMachine.GetComponent<NavMeshAgent>();
        }

        public override void ReceiveInteraction()
        {
            if (_dialogue == null)
            {
                GameLog.Error(this, "Dialogue is missing");
                return;
            }

            if (_characterStateMachine != null)
            {
                if (_characterStateMachine.CurrentState != _characterStateMachine.IdleCharacterState)
                    _lastState = _characterStateMachine.CurrentState;

                _characterStateMachine.SetState(_characterStateMachine.IdleCharacterState);
            }

            if (!_isLookingAtInteractor)
                StartCoroutine(LookAtInteractor());

            string name = _useNameReference ? _characterForName.Name : _characterStateMachine.Character.Name;
            CanvasManager.DialogCanvas.AddDialogue(_dialogue, name);
        }

        public override void UndoInteraction()
        {
            if (_characterStateMachine != null && _lastState != null)
                _characterStateMachine.SetState(_lastState);

            StopLookingAtInteractor();
            CanvasManager.DialogCanvas.ClearDialogue();
        }

        private IEnumerator LookAtInteractor()
        {
            _isLookingAtInteractor = true;

            while (_isLookingAtInteractor)
            {
                if (_currentInteractor != null)
                {
                    if (_navMeshAgent != null)
                        _navMeshAgent.updateRotation = false;

                    if (_useRotation)
                    {
                        Vector3 direction = _currentInteractor.transform.position - transform.parent.position;
                        direction.y = 0f;
                        if (direction != Vector3.zero)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            transform.parent.rotation = Quaternion.Slerp(
                                transform.parent.rotation,
                                targetRotation,
                                Time.deltaTime * _rotationSpeed
                            );
                        }
                    }
                }

                yield return null;
            }
        }

        private void StopLookingAtInteractor()
        {
            _isLookingAtInteractor = false;
            if (_currentInteractor != null)
            {
                if (_navMeshAgent != null)
                    _navMeshAgent.updateRotation = true;
            }
        }
    }
}