using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Base;

namespace Character.StateMachine.States
{
    public class IdleCharacterState : ICharacterState
    {
        public CharacterStates CharacterState => CharacterStates.Idle;

        public void EnterState(CharacterStateMachine character)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
}
