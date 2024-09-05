namespace Character.Class.Player
{
    public class PlayerClass : ICharacterClass
    {
        public int Id { get; }
        public string Name { get; }
        public int BaseHP { get; }
        public int BaseMana { get; }
        public int BaseStrength { get; }
        public int BaseDefense { get; }
        public int BaseAgility { get; }
        public int BaseDexterity { get; }
        public CharacterTypes Type { get; }
        
        public PlayerClass()
        {
            Id = 1;
            Name = "Player";
            BaseHP = 100;
            BaseMana = 50;
            BaseStrength = 10;
            BaseDefense = 5;
            BaseAgility = 5;
            BaseDexterity = 5;
            Type = CharacterTypes.NORMAL;
        }
    }
}