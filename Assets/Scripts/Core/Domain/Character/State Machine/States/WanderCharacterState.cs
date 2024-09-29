using UnityEngine;
using UnityEngine.AI;
using System;
using Character.Base;
using Random = UnityEngine.Random;

namespace Character.StateMachine.States
{
    [Serializable]
    public class WanderSettings
    {
        [Header("Wander")]
        public float WanderRadius = 10f;
        public float WanderTime = 5f;
        public float WanderSmoothDamp = .1f;
        [HideInInspector] public Vector3 Velocity = Vector3.zero;
    }

    public class WanderCharacterState : ICharacterState
    {
        private WanderSettings _wanderSettings;
        private NavMeshAgent _navMeshAgent;
        private CharacterStateMachine _character;
        private float _wanderTimer;
        private Vector3 _wanderTarget;
        private PursuitSettings _pursuitSettings;
        private Vector3 _startingPosition;
        private bool _reachedStartingPosition;

        public CharacterStates CharacterState => CharacterStates.Wander;

        #region Constructor
        public WanderCharacterState(WanderSettings wanderSettings, PursuitSettings pursuitSettings, Vector3 startingPosition)
        {
            _wanderSettings = wanderSettings;
            _pursuitSettings = pursuitSettings;
            _startingPosition = startingPosition;
        }
        #endregion

        #region Interface Methods Implementation
        public void EnterState(CharacterStateMachine character)
        {
            _character = character;
            _navMeshAgent ??= _character.GetComponent<NavMeshAgent>();

            _navMeshAgent.updatePosition = false;
            _navMeshAgent.isStopped = false;
            _wanderTimer = Random.Range(0f, _wanderSettings.WanderTime); // more organic

            _reachedStartingPosition = false;
            _navMeshAgent.SetDestination(_startingPosition);
        }

        public void UpdateState()
        {
            // when entering Wander, character first returns to start point
            if (!_reachedStartingPosition)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _reachedStartingPosition = true;
                    SetNewWanderTarget();
                }
                return;
            }

            _wanderTimer += Time.deltaTime;

            if (_wanderTimer >= _wanderSettings.WanderTime)
            {
                SetNewWanderTarget();
                _wanderTimer = 0f;
            }

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                _navMeshAgent.destination = _wanderTarget;

            // this avoids jittering on navmesh movement
            _navMeshAgent.transform.position = Vector3.SmoothDamp(_navMeshAgent.transform.position, _navMeshAgent.nextPosition,
                ref _wanderSettings.Velocity, _wanderSettings.WanderSmoothDamp);

            float distanceToTarget = _pursuitSettings.GetDistanceToTarget(_navMeshAgent.transform);
            if (distanceToTarget < _pursuitSettings.MaxPursuitDistance)
                _character.SetState(_character.PursuitCharacterState);
        }
        #endregion

        #region Helper Methods
        private void SetNewWanderTarget()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _wanderSettings.WanderRadius;
            randomDirection += _character.transform.position;

            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, _wanderSettings.WanderRadius, -1);

            _wanderTarget = navHit.position;
            _navMeshAgent.SetDestination(_wanderTarget);
        }
        #endregion
    }
}
