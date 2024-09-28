using UnityEngine;
using UnityEngine.AI;
using Character.Base;

namespace Character.StateMachine.States
{
    public class FollowCharacterState : ICharacterState
    {
        #region NavMesh Follow
        private Vector3 _offset = new Vector3(0, 0, 2f);
        private float _smoothDampTime = .1f;
        private Vector3 _velocity = Vector3.zero;
        #endregion

        #region NavMesh Out of the way
        private float _minDistanceToPlayer = 2f;
        private float _moveAwayDistance = 1.8f;
        #endregion

        #region NavMesh Warp
        private float _maxDistanceToPlayer = 20f;
        #endregion

        #region References
        private Player.Player _player;
        private NavMeshAgent _navMeshAgent;
        private BaseCharacter _character;
        #endregion

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
            if (distanceToPlayer < _minDistanceToPlayer)
            {
                // move away if too close
                Vector3 moveAwayDirection = (_navMeshAgent.transform.position - _player.transform.position).normalized;
                _navMeshAgent.destination = _navMeshAgent.transform.position + moveAwayDirection * _moveAwayDistance;
            }
            else if (distanceToPlayer > _maxDistanceToPlayer)
            {
                // warp to player
                Vector3 direction = (_navMeshAgent.transform.position - _player.transform.position).normalized;
                _navMeshAgent.Warp(_player.transform.position + direction * _offset.magnitude);
            }
            else
            {
                // follow player
                Vector3 direction = (_navMeshAgent.transform.position - _player.transform.position).normalized;
                _navMeshAgent.destination = _player.transform.position + direction * _offset.magnitude;
            }

            _navMeshAgent.transform.position = Vector3.SmoothDamp(_navMeshAgent.transform.position, _navMeshAgent.nextPosition, ref _velocity, _smoothDampTime);
        }
    }
}
