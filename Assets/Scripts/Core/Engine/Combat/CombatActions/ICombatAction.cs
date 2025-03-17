using System.Collections.Generic;
using Character.Base;
using Core.Domain.Character.Moves;

namespace Combat
{
    public interface ICombatAction
    {
        void Execute(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets);
    }
}