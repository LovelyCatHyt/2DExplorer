using System;
using GameFramework.Fsm;

namespace Game
{
    public class InvalidGameStateTargetException : Exception
    {
        public Type sourceType;
        public Type targetType;

        public override string Message => $"Invalid game state changing from {sourceType.Name} to {targetType.Name}!";

        public InvalidGameStateTargetException(Type source, Type target)
        {
            sourceType = source;
            targetType = target;
        }
    }

    /// <summary>
    /// 游戏状态基类
    /// </summary>
    public abstract class GameBaseState : FsmState<GameInstance>
    {
        /// <summary>
        /// 当前状态对应的枚举量
        /// </summary>
        public abstract GameState StateEnum { get; }

        /// <summary>
        /// 尝试改变状态, 成功则返回 true
        /// </summary>
        /// <typeparam name="T">目标状态的类型</typeparam>
        /// <returns>成功改变状态</returns>
        public abstract bool TryChangeState<T>(IFsm<GameInstance> fsm) where T : GameBaseState;

        public virtual void ChangeGameState<T>(IFsm<GameInstance> fsm) where T : GameBaseState
        {
            if (!TryChangeState<T>(fsm)) throw new InvalidGameStateTargetException(this.GetType(), typeof(T));
        }

    }

    public static class GameStateHelper
    {
        public static GameBaseState GameState(this IFsm<GameInstance> fsm) => (GameBaseState)fsm.CurrentState;

        public static void ChangeGameState<T>(this IFsm<GameInstance> fsm) where T : GameBaseState =>
            fsm.GameState().ChangeGameState<T>(fsm);

        public static bool TryChangeGameState<T>(this IFsm<GameInstance> fsm) where T : GameBaseState =>
            fsm.GameState().TryChangeState<T>(fsm);
    }
}
