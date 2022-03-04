using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Fsm;
using TileDataIO;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;
using Zenject;

namespace Game
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// 未进入游戏
        /// </summary>
        NotEntered,
        /// <summary>
        /// 正在载入地图
        /// </summary>
        Loading,
        /// <summary>
        /// 游戏中
        /// </summary>
        InGame,
        /// <summary>
        /// 暂停
        /// </summary>
        Paused,
        /// <summary>
        /// 正在保存地图
        /// </summary>
        Saving
    }

    /// <summary>
    /// 游戏状态
    /// </summary>
    [DisallowMultipleComponent]
    public partial class GameInstance : MonoBehaviour
    {
        [Serializable]
        public class StateChangeEvents
        {
            /// <summary>
            /// 游戏开始事件, 一局游戏只会触发一次
            /// </summary>
            public UnityEvent onGameStart;
            /// <summary>
            /// 加载前事件
            /// </summary>
            public UnityEvent onBeforeLoading;
            /// <summary>
            /// 加载完成但没进入游戏的事件
            /// </summary>
            public UnityEvent onLoadFinished;
            /// <summary>
            /// 保存前事件
            /// </summary>
            public UnityEvent onBeforeSave;
            /// <summary>
            /// 保存完成事件
            /// </summary>
            public UnityEvent onSaveFinished;
            /// <summary>
            /// 暂停前事件
            /// </summary>
            public UnityEvent onBeforePause;
            /// <summary>
            /// 暂停恢复事件
            /// </summary>
            public UnityEvent onPauseResumed;
        }

        public StateChangeEvents events = new StateChangeEvents();

        [Inject] private FsmComponent _fsmComponent;
        [Inject] private TileDataMgr _tileDataMgr;
        private IFsm<GameInstance> _fsm;
        
        private void Start()
        {
            _fsm = _fsmComponent.CreateFsm(this,
                new NotEntered(),
                new Loading(),
                new InGame(),
                new Paused(),
                new Saving()
            );
            _fsm.Start<NotEntered>();
            _fsm.ChangeGameState<InGame>();
        }

        public void LoadGame()
        {
            _fsm.ChangeGameState<Loading>();
            _tileDataMgr.LoadWholeGrid();
            _fsm.ChangeGameState<InGame>();
        }
    }

}
