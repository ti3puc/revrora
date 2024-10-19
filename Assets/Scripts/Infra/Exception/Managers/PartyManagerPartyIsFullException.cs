namespace Infra.Exception.Managers
{
    public class PartyManagerPartyIsFullException : GeneralGameException<PartyManagerPartyIsFullException>
    {
        public PartyManagerPartyIsFullException(string message = "Party is full!") : base(message)
        {
            
        }
    }
}