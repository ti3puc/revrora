namespace Infra.Exception.Character
{
    public class CharacterStateMachineNotInitializedException : GeneralGameException<CharacterStateMachineNotInitializedException>
    {
        public CharacterStateMachineNotInitializedException(string stateMachineObjName, string message = "State machine not initialized.") : base(message)
        {

        }
    }
}