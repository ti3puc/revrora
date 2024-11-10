using System.Collections.Generic;
using Character.Base;
using Core.Domain.Character.Moves;

namespace Combat
{
    public interface ICombatAction
    {
        void execute(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets);
    }
}