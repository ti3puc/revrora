namespace Infra.Exception.Managers
{
    public class PartyManagerGeneralException : GeneralGameException<PartyManagerGeneralException>
    {
        public PartyManagerGeneralException(string message = "Unknow Error on PartyManager!") : base(message)
        {
            
        }
    }
}