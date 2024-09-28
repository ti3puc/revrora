using Character.Base;
using Character.Class;
using Character.StateMachine.States;
using Infra.Exception.Character;
using UnityEngine;

namespace Character.StateMachine
{
    public class CharacterStateMachine : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        [SerializeField] private BaseCharacter _character;

        private ICharacterState _currentState;

        private IdleCharacterState _idleCharacterState;
        private WanderCharacterState _wanderCharacterState;
        private FollowCharacterState _followCharacterState;
        private PursuitCharacterState _pursuitCharacterState;
        #endregion

        #region Properties
        public IdleCharacterState IdleCharacterState => _idleCharacterState;
        public WanderCharacterState WanderCharacterState => _wanderCharacterState;
        public FollowCharacterState FollowCharacterState => _followCharacterState;
        public PursuitCharacterState PursuitCharacterState => _pursuitCharacterState;
        #endregion

        #region Unity Cycle
        private void Awake()
        {
            // initialize states
            _idleCharacterState = new IdleCharacterState();
            _wanderCharacterState = new WanderCharacterState();
            _followCharacterState = new FollowCharacterState();
            _pursuitCharacterState = new PursuitCharacterState();

            // set initial state
            SetState(_followCharacterState);
        }

        private void FixedUpdate()
        {
            if (_currentState == null) return;
            _currentState.UpdateState();
        }
        #endregion

        #region Public Methods
        public void SetState(ICharacterState newState)
        {
            if (newState == null)
                throw new InvalidCharacterStateException(name + ": trying to change to invalid state", this);

            _currentState = newState;
            _currentState.EnterState(_character);
        }
        #endregion
    }
}
