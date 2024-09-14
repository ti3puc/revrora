namespace Infra.Exception.Managers
{
    public class PartyManagerCharacterNotInPartyException : GeneralGameException<PartyManagerCharacterNotInPartyException>
    {
        public PartyManagerCharacterNotInPartyException(string message = "Character is not in party!") : base(message)
        {
            
        }
    }
}