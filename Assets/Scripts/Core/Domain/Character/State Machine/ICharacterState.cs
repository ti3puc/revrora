using Character.Base;

namespace Character.StateMachine
{
    public enum CharacterStates
    {
        Idle,
        Wander,
        Follow,
        Pursuit,
    }

    public interface ICharacterState
    {
        CharacterStates CharacterState { get; }
        void EnterState(BaseCharacter character);
        void UpdateState();
    }
}
