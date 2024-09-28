using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Base;

namespace Character.StateMachine.States
{
    public class WanderCharacterState : ICharacterState
    {
        public CharacterStates CharacterState => CharacterStates.Wander;

        public void EnterState(BaseCharacter character)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
}
