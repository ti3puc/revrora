namespace Infra.Exception.Character
{
    public class InvalidCharacterStateException : GeneralGameException<InvalidCharacterStateException>
    {
        public InvalidCharacterStateException(string message, UnityEngine.Object context) : base(message, context)
        {

        }
    }
}