using Character.Base;

namespace Character.StateMachine
{
    public interface ICharacterState
    {
        void EnterState(BaseCharacter character);
        void UpdateState();
    }
}
