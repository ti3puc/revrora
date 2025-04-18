using UnityEngine;
using UnityEngine.AI;
using System;
using Character.Base;
using Infra.Exception.Character;
using NaughtyAttributes;
using Managers.Player;

namespace Character.StateMachine.States
{
    [Serializable]
    public class PursuitSettings
    {
        [Header("Pursuit")]
        public bool PursuitPlayer = true;
        [HideIf("PursuitPlayer")] public Transform PursuitTarget;
        public float MaxPursuitDistance = 30f;
        public float PursuitSmoothDamp = .1f;
        [HideInInspector] public Vector3 Velocity = Vector3.zero;

        public float GetDistanceToTarget(Transform character)
        {
            if (PursuitPlayer)
                PursuitTarget = PlayerManager.Instance.Player.transform;

            if (PursuitTarget == null)
                throw new InvalidFollowTargetException(character.name + ": missing follow target reference", character);

            return Vector3.Distance(character.position, PursuitTarget.position);
        }
    }

    public class PursuitCharacterState : ICharacterState
    {
        private PursuitSettings _pursuitSettings;
        private NavMeshAgent _navMeshAgent;
        private CharacterStateMachine _character;

        public CharacterStates CharacterState => CharacterStates.Pursuit;

        #region Constructor
        public PursuitCharacterState(PursuitSettings pursuitSettings)
        {
            _pursuitSettings = pursuitSettings;
        }
        #endregion

        #region Interface Methods Implementation
        public void EnterState(CharacterStateMachine character)
        {
            _character = character;
            _navMeshAgent ??= _character.GetComponent<NavMeshAgent>();

            _navMeshAgent.updatePosition = false;

            if (_pursuitSettings.PursuitPlayer)
                _pursuitSettings.PursuitTarget = PlayerManager.Instance.Player.transform;

            if (_pursuitSettings.PursuitTarget == null)
                throw new InvalidFollowTargetException(_character.name + ": missing follow target reference", _character);
        }

        public void UpdateState()
        {
            if (_pursuitSettings.PursuitTarget == null) return;

            float distanceToTarget = _pursuitSettings.GetDistanceToTarget(_navMeshAgent.transform);
            if (distanceToTarget > _pursuitSettings.MaxPursuitDistance)
            {
                _navMeshAgent.isStopped = true;
                _character.SetState(_character.WanderCharacterState);
            }
            else
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_pursuitSettings.PursuitTarget.position);
            }

            // this avoids jittering on navmesh movement
            _navMeshAgent.transform.position = Vector3.SmoothDamp(_navMeshAgent.transform.position, _navMeshAgent.nextPosition,
                ref _pursuitSettings.Velocity, _pursuitSettings.PursuitSmoothDamp);
        }
        #endregion
    }
}
