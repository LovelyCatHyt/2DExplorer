using System;
using Audio;
using CharCtrl;
using Unitilities;
using UnityEngine;
using Zenject;

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
        /// <summary>
        /// 子弹的伤害
        /// </summary>
        public double damage = 10;
        public Vector3 direction;
        /// <summary>
        /// 撞击事件
        /// </summary>
        public event Action onHit;
        /// <summary>
        /// 有效攻击事件
        /// </summary>
        public event Action<GameObject> onValidHit;
        public AudioClip hitSound;

        [Inject(Id = "Explosion Pool")]
        private GameObjectPool _explosionPool;
        [Inject(Id = "Bullet Pool")]
        private GameObjectPool _bulletPool;
        [Inject]
        private AudioManager _audioManager;
        /// <summary>
        /// 发射这个子弹的炮塔的碰撞体
        /// </summary>
        private Collider2D _turretCollider;

        public virtual void Init(Vector3 position, Vector3 target, Collider2D fromTurret)
        {
            transform.position = position;
            direction = (target - position).normalized;
            _turretCollider = fromTurret;
        }
        
        private void FixedUpdate()
        {
            transform.position += direction * (speed * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider == _turretCollider) return;
            onHit?.Invoke();
            var role = other.collider.GetComponent<IRole>();
            if (role != null)
            {
                onValidHit?.Invoke(other.collider.gameObject);
                role.Damaged(damage);
                Debug.Assert(_explosionPool.Pop(out var effect));
                effect.transform.SetPosition2D(transform.position);
            }
            onHit = null;
            onValidHit = null;
            _bulletPool.Push(gameObject);
            _audioManager.PlaySound(_audioManager.GetTrackIDFromName("Sound"), hitSound, transform.position);
        }
    }
}
