using Audio;
using Effect;
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
        public double Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health < 0)
                {
                    Dead();
                }

                if (_health > initHealth)
                {
                    _health = initHealth;
                }
                _hpEffect.Hp = (float)(_health / initHealth);
            }
        }
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
        [SerializeField] private HpEffect _hpEffect;
        [SerializeField] private double _health = 100;

        private void Awake()
        {
            startCheckPoint.connectedChar = this;
            currentCheckPoint = startCheckPoint;
            _game.events.onGameStart.AddListener(Init);
            _game.events.onLoadFinished.AddListener(Init);
        }

        private void Init()
        {
            Health = initHealth;
            transform.SetPosition2D(currentCheckPoint.transform.position);
        }

        public void HealDirectly(double value)
        {
            Health += value;
        }

        public void HealByPercent(double value)
        {
            Health += initHealth * value;
        }

        public void Damaged(double value)
        {
            Health -= value;
        }

        public void Dead()
        {
            _audioManager.PlaySound(_audioManager.GetTrackIDFromName("BackGround"), deadAudio);
            Init();
        }
    }

}
