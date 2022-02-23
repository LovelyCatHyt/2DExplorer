using System;
using CharCtrl;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 子弹
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <summary>
        /// 子弹速度
        /// </summary>
        public float speed = 1;
        public Vector3 direction;
        /// <summary>
        /// 撞击事件
        /// </summary>
        public event Action onHit;
        /// <summary>
        /// 有效攻击事件
        /// </summary>
        public event Action<GameObject> onValidHit;

        public virtual void Init(Vector3 position, Vector3 target)
        {
            transform.position = position;
            direction = (target - position).normalized;
        }

        private void FixedUpdate()
        {
            transform.position += direction * (speed * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            onHit?.Invoke();
            if (other.collider.GetComponent<MainCharCtrl>())
            {
                onValidHit?.Invoke(other.collider.gameObject);
            }
            onHit = null;
            onValidHit = null;
        }
    }
}
