namespace Character.Class
{
    public interface ICharacterClass
    {
        int Id { get; }
        string Name { get; }
        int BaseHP { get; }
        int BaseMana { get; }
        int BaseStrength { get; }
        int BaseDefense { get; }
        int BaseAgility { get; }
        int BaseWisdom { get; }
        CharacterTypes Type { get; }
    }
}