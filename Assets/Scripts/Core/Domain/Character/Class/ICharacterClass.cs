using Character.Base;

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

        public void RaiseCharacterDied();
        public void RaiseDamageReceived();
    }
}