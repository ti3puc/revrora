using Character.Base;
using UnityEngine;

namespace Character.Class
{
    public interface ICharacterClass
    {
        int Id { get; }
        string Name { get; }
        int HP { get; }
        int MaxHP { get; }
        int Attack { get; }
        int Defense { get; }
        int Speed { get; }
        int Intelligence { get; }
        CharacterDefinition CharacterDefinition { get; set; }
        CharacterTeam CharacterTeam { get; }
        int CustomLevel { get; set; }

        public void RaiseCharacterDied();
        public void RaiseDamageReceived(GameObject vfx);
        public void RaiseHealReceived(GameObject vfx);
        public void RaiseImprovedStat(GameObject vfx);
        public void RaiseDamageMissed();
    }
}