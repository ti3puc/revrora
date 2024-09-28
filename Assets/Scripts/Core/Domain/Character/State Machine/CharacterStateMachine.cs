using System.Collections;
using System.Collections.Generic;
using Character.Class;
using Infra.Exception.Character;
using NaughtyAttributes;
using UnityEngine;

namespace Character.StateMachine
{
    public class CharacterStateMachine : MonoBehaviour
    {
        #region Fields
        private ICharacterClass _character;
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
        private void Update()
        {
            _currentState.UpdateState();
        }
        #endregion

        #region Public Methods
        public void InitializeStateMachine(ICharacterClass character)
        {
            _character = character;

            // initialize states
            _idleCharacterState = new IdleCharacterState();
            _wanderCharacterState = new WanderCharacterState();
            _followCharacterState = new FollowCharacterState();
            _pursuitCharacterState = new PursuitCharacterState();

            // set initial state
            SetState(_idleCharacterState);
        }

        public void SetState(ICharacterState newState)
        {
            if (_character == null)
            {
                throw new CharacterStateMachineNotInitializedException(name);
            }

            _currentState = newState;
            _currentState.EnterState(_character);
        }
        #endregion
    }
}
