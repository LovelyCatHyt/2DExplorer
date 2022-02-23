using Unitilities;
using UnityEngine;

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

        private void Awake()
        {
            startCheckPoint.connectedChar = this;
            currentCheckPoint = startCheckPoint;
        }

        private void Init()
        {
            health = initHealth;
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
            transform.SetPosition2D(currentCheckPoint.transform.position);
            Init();
        }
    }

}
