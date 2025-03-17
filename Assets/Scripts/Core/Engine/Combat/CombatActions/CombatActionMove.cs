using System.Collections.Generic;
using Character.Base;
using Combat;
using Core.Domain.Character.Moves;
using UnityEngine;

namespace Core.Engine.Combat.CombatActions
{
    public class CombatActionMove : ICombatAction
    {
        private List<BaseCharacter> _targets;

        public void Execute(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            _targets = targets;

            switch (move.Category)
            {
                case MoveCategory.PHYSICAL:
                    DoPhysicalMove(user, move, targets);
                    break;
                case MoveCategory.STATUS:
                    DoStatusMove(user, move, targets);
                    break;
            }
        }

        private void DoPhysicalMove(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            foreach (var target in targets)
            {
                var random = Random.Range(0, 4);
                var damage = (user.CharacterStats.Attack + move.Power + random) - target.CharacterStats.Defense;

                Debug.Log($"Damage: ({user.CharacterStats.Attack} + {move.Power} + {random}) - {target.CharacterStats.Defense} = {damage}");

                target.CharacterStats.ReceiveDamage(damage);
            }
        }

        private void DoStatusMove(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {

        }
    }
}