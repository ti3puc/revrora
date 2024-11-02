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
        }

        public void UpdateState()
        {
        }
    }
}
