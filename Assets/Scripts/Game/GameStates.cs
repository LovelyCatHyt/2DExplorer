using GameFramework.Fsm;

namespace Game
{
    public partial class GameInstance
    {
        public class NotEntered : GameBaseState
        {
            public override GameState StateEnum => GameState.NotEntered;
            public override bool TryChangeState<T>(IFsm<GameInstance> fsm)
            {
                var type = typeof(T);
                if (type == typeof(NotEntered)) return true;
                if (type != typeof(InGame)) return false;
                fsm.Owner.events.onGameStart?.Invoke();
                ChangeState<InGame>(fsm);
                return true;
            }
        }

        public class Loading : GameBaseState
        {
            public override GameState StateEnum => GameState.Loading;

            protected override void OnEnter(IFsm<GameInstance> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.events.onBeforeLoading?.Invoke();
            }

            protected override void OnLeave(IFsm<GameInstance> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                fsm.Owner.events.onLoadFinished?.Invoke();
            }

            public override bool TryChangeState<T>(IFsm<GameInstance> fsm)
            {
                var type = typeof(T);
                if (type == typeof(Loading)) return true;
                if (type == typeof(InGame))
                {
                    ChangeState<InGame>(fsm);
                    return true;
                }
                return false;
            }
        }

        public class InGame : GameBaseState
        {
            public override GameState StateEnum => GameState.InGame;
            public override bool TryChangeState<T>(IFsm<GameInstance> fsm)
            {
                var type = typeof(T);
                if (type == typeof(InGame)) return true;

                if (type == typeof(Paused) ||
                    type == typeof(Loading) ||
                    type == typeof(Saving))
                {
                    ChangeState<T>(fsm);
                    return true;
                }

                return false;
            }
        }

        public class Paused : GameBaseState
        {
            public override GameState StateEnum => GameState.Paused;

            protected override void OnEnter(IFsm<GameInstance> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.events.onBeforePause?.Invoke();
            }

            protected override void OnLeave(IFsm<GameInstance> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                fsm.Owner.events.onPauseResumed?.Invoke();
            }

            public override bool TryChangeState<T>(IFsm<GameInstance> fsm)
            {
                var type = typeof(T);
                if (type == typeof(Paused)) return true;
                if (type == typeof(InGame))
                {
                    ChangeState<InGame>(fsm);
                    return true;
                }
                return false;
            }
        }

        public class Saving : GameBaseState
        {
            public override GameState StateEnum => GameState.Saving;

            protected override void OnEnter(IFsm<GameInstance> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.events.onBeforeSave?.Invoke();
            }

            protected override void OnLeave(IFsm<GameInstance> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                fsm.Owner.events.onSaveFinished?.Invoke();
            }

            public override bool TryChangeState<T>(IFsm<GameInstance> fsm)
            {
                var type = typeof(T);
                if (type == typeof(Saving)) return true;
                if (type == typeof(InGame))
                {
                    ChangeState<InGame>(fsm);
                    return true;
                }

                return false;
            }
        }
    }
}
