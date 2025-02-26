using Cysharp.Threading.Tasks;
using Zenject;

namespace Redpenguin.Core.StateManagement
{
    public class GameStateMachine : IGameStateMachine, ITickable
    {
        private IExitableState _activeState;
        private readonly IStateFactory _stateFactory;

        public GameStateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void Tick()
        {
            if (_activeState is IUpdateable updateableState)
                updateableState.Update();
        }

        public void Enter<TState>() where TState : class, IState =>
            RequestEnter<TState>()
                .Forget();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload> =>
            RequestEnter<TState, TPayload>(payload)
                .Forget();

        public UniTask EnterAsync<TState>() where TState : class, IState
        {
            return RequestEnter<TState>();
        }

        public UniTask EnterAsync<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            return RequestEnter<TState, TPayload>(payload);
        }

        private UniTask<TState> RequestEnter<TState>() where TState : class, IState =>
            RequestChangeState<TState>()
                .ContinueWith(EnterState);

        private UniTask<TState> RequestEnter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadState<TPayload> =>
            RequestChangeState<TState>()
                .ContinueWith(state => EnterPayloadState(state, payload));

        private async UniTask<TState> EnterState<TState>(TState state) where TState : class, IState
        {
            _activeState = state;
            if (_activeState is IStateAsync stateAsync)
                await stateAsync.EnterAsync();
            else
                state.Enter();
            return state;
        }

        private async UniTask<TState> EnterPayloadState<TState, TPayload>(TState state, TPayload payload)
            where TState : class, IPayloadState<TPayload>
        {
            _activeState = state;
            if (_activeState is IPayloadStateAsync<TPayload> stateAsync)
                await stateAsync.EnterAsync(payload);
            else
                state.Enter(payload);
            return state;
        }

        private UniTask<TState> RequestChangeState<TState>() where TState : class, IExitableState
        {
            if (_activeState != null)
            {
                return _activeState
                    .BeginExit()
                    .ContinueWith(_activeState.EndExit)
                    .ContinueWith(ChangeState<TState>);
            }

            return ChangeState<TState>();
        }


        private UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            TState state = _stateFactory.GetState<TState>();
            return UniTask.FromResult(state);
        }
    }
}