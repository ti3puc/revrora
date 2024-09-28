using Character.Class;

namespace Character.StateMachine
{
    public interface ICharacterState
    {
        void EnterState(ICharacterClass character);
        void UpdateState();
    }
}
