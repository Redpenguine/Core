namespace Redpenguin.StateManagement.Default
{
    public static class StateMachineExtensions
    {
        public static bool Is<T>(this IStateMachine stateMachine) where T : IState
        {
            return stateMachine.CurrentState is T;
        }
    }
}