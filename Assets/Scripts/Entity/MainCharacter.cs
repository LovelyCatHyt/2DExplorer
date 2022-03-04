using Audio;
using Game;
using Unitilities;
using UnityEngine;
using Zenject;

namespace Entity
{
    /// <summary>
    /// 主角, 即玩家
    /// </summary>
    public class MainCharacter : MonoBehaviour, IRole
    {
        public double initHealth = 100;
        public double health = 100;
        /// <summary>
        /// 游戏开始时的检查点
        /// </summary>
        public CheckPoint startCheckPoint;
        /// <summary>
        /// 当前检查点
        /// </summary>
        public CheckPoint currentCheckPoint;
        public AudioClip deadAudio;

        [Inject] private GameInstance _game;
        [Inject] private AudioManager _audioManager;

        private void Awake()
        {
            startCheckPoint.connectedChar = this;
            currentCheckPoint = startCheckPoint;
            _game.events.onGameStart.AddListener(Init);
            _game.events.onLoadFinished.AddListener(Init);
        }

        private void Init()
        {
            health = initHealth;
            transform.SetPosition2D(currentCheckPoint.transform.position);
        }

        public void Damaged(double value)
        {
            health -= value;
            if (health < 0)
            {
                Dead();
            }
        }

        public void Dead()
        {
            _audioManager.PlaySound(_audioManager.GetTrackIDFromName("BackGround"), deadAudio);
            Init();
        }
    }

}
