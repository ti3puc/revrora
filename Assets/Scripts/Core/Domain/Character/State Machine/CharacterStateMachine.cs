using Character.Base;
using Character.Class;
using Character.StateMachine.States;
using Infra.Exception.Character;
using NaughtyAttributes;
using UnityEngine;

namespace Character.StateMachine
{
    public class CharacterStateMachine : MonoBehaviour
    {
        #region Fields
        [Header("Settings")]
        [SerializeField] private CharacterStates _initialState = CharacterStates.Idle;
        [SerializeField] private FollowSettings _followStateSettings;
        [SerializeField] private WanderSettings _wanderStateSettings;
        [SerializeField] private PursuitSettings _pursuitStateSettings;
        [Header("References")]
        [SerializeField] private BaseCharacter _character;
        [Header("Debug")]
        [SerializeField, ReadOnly] private CharacterStates _currentState;

        private ICharacterState _actualCurrentState;

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
            if (_character == null)
                throw new InvalidCharacterException(name + ": missing base character reference", this);

            // initialize states
            _idleCharacterState = new IdleCharacterState();
            _wanderCharacterState = new WanderCharacterState(_wanderStateSettings, _pursuitStateSettings, transform.position);
            _followCharacterState = new FollowCharacterState(_followStateSettings);
            _pursuitCharacterState = new PursuitCharacterState(_pursuitStateSettings);

            // set initial state
            ICharacterState firstState = _initialState switch
                {
                    CharacterStates.Idle => _idleCharacterState,
                    CharacterStates.Wander => _wanderCharacterState,
                    CharacterStates.Follow => _followCharacterState,
                    CharacterStates.Pursuit => _idleCharacterState,
                    _ => IdleCharacterState,
                };

            SetState(firstState);
        }

        private void FixedUpdate()
        {
            if (_actualCurrentState == null) return;
            _actualCurrentState.UpdateState();
        }
        #endregion

        #region Public Methods
        public void SetState(ICharacterState newState)
        {
            if (newState == null)
                throw new InvalidCharacterStateException(name + ": trying to change to invalid state", this);

            _actualCurrentState = newState;
            _actualCurrentState.EnterState(this);
            _currentState = _actualCurrentState.CharacterState;
        }
        #endregion
    }
}
