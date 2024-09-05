namespace Character.Class.Player
{
    public class Class001Player : ICharacterClass
    {
        public int Id { get; }
        public string Name { get; }
        public int BaseHP { get; }
        public int BaseMana { get; }
        public int BaseStrength { get; }
        public int BaseDefense { get; }
        public int BaseAgility { get; }
        public int BaseWisdom { get; }
        public CharacterTypes Type { get; }
        
        public Class001Player()
        {
            // Atributos base de ainda de teste, seguem os valores base do pokémon charmander
            Id = 1;
            Name = "Player";
            BaseHP = 39;
            BaseMana = 25;
            BaseStrength = 52;
            BaseDefense = 43;
            BaseAgility = 65;
            BaseWisdom = 62;
            Type = CharacterTypes.NORMAL;
        }
    }
}