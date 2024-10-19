namespace Infra.Exception.Character
{
    public class InvalidFollowTargetException : GeneralGameException<InvalidCharacterStateException>
    {
        public InvalidFollowTargetException(string message, UnityEngine.Object context) : base(message, context)
        {

        }
    }
}