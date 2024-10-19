namespace Infra.Exception.Managers
{
    public class PartyManagerPartyIsEmptyException : GeneralGameException<PartyManagerPartyIsEmptyException>
    {
        public PartyManagerPartyIsEmptyException(string message = "Party is empty!") : base(message)
        {
            
        }
    }
}