namespace Redpenguin.StateManagement.TransitionsSeparately
{
    public interface IState
    {
        void OnStay();
        void OnEnter();
        void OnExit();
    }
}