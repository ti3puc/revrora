namespace Infra.Exception.Character
{
    public class InvalidCharacterException : GeneralGameException<InvalidCharacterStateException>
    {
        public InvalidCharacterException(string message, UnityEngine.Object context) : base(message, context)
        {

        }
    }
}