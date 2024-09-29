using UnityEngine;
using UnityEngine.AI;
using Character.Base;
using System;
using NaughtyAttributes;
using Player;
using Managers.Player;
using Infra.Exception.Character;

namespace Character.StateMachine.States
{
    [Serializable]
    public class FollowSettings
    {
        [Header("Follow")]
        public Transform FollowTarget;
        public Vector3 FollowOffset = new Vector3(0, 0, 2f);
        public float FollowSmoothDamp = .1f;
        [HideInInspector] public Vector3 Velocity = Vector3.zero;

        [Header("Out of the way")]
        public float MinDistanceToPlayer = 2f;
        public float MoveAwayDistance = 1.8f;

        [Header("Warp")]
        public float MaxDistanceToPlayer = 20f;
    }

    public class FollowCharacterState : ICharacterState
    {
        private FollowSettings _followSettings;
        private NavMeshAgent _navMeshAgent;
        private CharacterStateMachine _character;

        public CharacterStates CharacterState => CharacterStates.Follow;

        #region Constructor
        public FollowCharacterState(FollowSettings followSettings)
        {
            _followSettings = followSettings;
        }
        #endregion

        #region Interface Methods Implementation
        public void EnterState(CharacterStateMachine character)
        {
            _character = character;
            _navMeshAgent ??= _character.GetComponent<NavMeshAgent>();

            _navMeshAgent.updatePosition = false;

            if (_followSettings.FollowTarget == null)
                throw new InvalidFollowTargetException(_character.name + ": missing follow target reference", _character);
        }

        public void UpdateState()
        {
            if (_followSettings.FollowTarget == null) return;

            float distanceToPlayer = Vector3.Distance(_navMeshAgent.transform.position, _followSettings.FollowTarget.position);
            if (distanceToPlayer < _followSettings.MinDistanceToPlayer)
            {
                // move away if too close
                Vector3 moveAwayDirection = (_navMeshAgent.transform.position - _followSettings.FollowTarget.position).normalized;
                _navMeshAgent.destination = _navMeshAgent.transform.position + moveAwayDirection * _followSettings.MoveAwayDistance;
            }
            else if (distanceToPlayer > _followSettings.MaxDistanceToPlayer)
            {
                // warp to player
                Vector3 direction = (_navMeshAgent.transform.position - _followSettings.FollowTarget.position).normalized;
                _navMeshAgent.Warp(_followSettings.FollowTarget.position + direction * _followSettings.FollowOffset.magnitude);
            }
            else
            {
                // follow player
                Vector3 direction = (_navMeshAgent.transform.position - _followSettings.FollowTarget.position).normalized;
                _navMeshAgent.destination = _followSettings.FollowTarget.position + direction * _followSettings.FollowOffset.magnitude;
            }

            // this avoids jittering on navmesh movement
            _navMeshAgent.transform.position = Vector3.SmoothDamp(_navMeshAgent.transform.position, _navMeshAgent.nextPosition,
                ref _followSettings.Velocity, _followSettings.FollowSmoothDamp);
        }
        #endregion
    }
}
