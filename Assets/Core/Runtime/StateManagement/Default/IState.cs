namespace Redpenguin.StateManagement.Default
{
    public interface IState : IStateBase
    {
        void Enter();
    }
    
    public interface IPayloadedState<TPayload> : IStateBase
    {
        void Enter(TPayload payload);
    }

    public interface IStateBase
    {
        void Exit();
    }
}