using UnityEngine;
using UnityEngine.AI;
using Character.Base;
using System;
using NaughtyAttributes;

namespace Character.StateMachine.States
{
    [Serializable]
    public class FollowSettings
    {
        [Header("Follow")]
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
        private Player.Player _player;
        private NavMeshAgent _navMeshAgent;
        private BaseCharacter _character;

        public CharacterStates CharacterState => CharacterStates.Follow;

        #region Constructor
        public FollowCharacterState(FollowSettings followSettings)
        {
            _followSettings = followSettings;
        }
        #endregion

        #region Interface Methods Implementation
        public void EnterState(BaseCharacter character)
        {
            _character = character;
            _navMeshAgent ??= _character.GetComponent<NavMeshAgent>();
            _player ??= GameObject.FindObjectOfType<Player.Player>();

            _navMeshAgent.updatePosition = false;
        }

        public void UpdateState()
        {
            if (_player == null) return;

            float distanceToPlayer = Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position);
            if (distanceToPlayer < _followSettings.MinDistanceToPlayer)
            {
                // move away if too close
                Vector3 moveAwayDirection = (_navMeshAgent.transform.position - _player.transform.position).normalized;
                _navMeshAgent.destination = _navMeshAgent.transform.position + moveAwayDirection * _followSettings.MoveAwayDistance;
            }
            else if (distanceToPlayer > _followSettings.MaxDistanceToPlayer)
            {
                // warp to player
                Vector3 direction = (_navMeshAgent.transform.position - _player.transform.position).normalized;
                _navMeshAgent.Warp(_player.transform.position + direction * _followSettings.FollowOffset.magnitude);
            }
            else
            {
                // follow player
                Vector3 direction = (_navMeshAgent.transform.position - _player.transform.position).normalized;
                _navMeshAgent.destination = _player.transform.position + direction * _followSettings.FollowOffset.magnitude;
            }

            _navMeshAgent.transform.position = Vector3.SmoothDamp(_navMeshAgent.transform.position, _navMeshAgent.nextPosition,
                ref _followSettings.Velocity, _followSettings.FollowSmoothDamp);
        }
        #endregion
    }
}
