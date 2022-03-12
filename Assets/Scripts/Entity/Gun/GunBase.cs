using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Decorator;
using Entity.Factory;
using Unitilities;
using Unitilities.PropAttr;
using UnityEngine;
using Zenject;

namespace Entity.Gun
{
    /// <summary>
    /// 枪的基类
    /// </summary>
    public class GunBase : MonoBehaviour
    {
        public string bulletType;
        [SerializeField] private Collider2D _turretCollider;
        [Inject] private BulletFactory _factory;
        [SerializeField] protected TurretAnim _anim;

        protected virtual void Awake()
        {
            if (!_turretCollider)
            {
                _turretCollider = GetComponent<Collider2D>();
            }
            if (!_anim) _anim = GetComponent<TurretAnim>();
        }

        /// <summary>
        /// 朝目标开火
        /// </summary>
        /// <param name="targetTran">目标的 <see cref="Transform"/></param>
        public virtual void Fire(Transform targetTran) => Fire(targetTran.position);

        /// <summary>
        /// 朝目标开火
        /// </summary>
        /// <param name="target">目标位置</param>
        public virtual void Fire(Vector2 target)
        {
            _factory.CreateBullet(bulletType, transform.Position2D(), target, _turretCollider);
            _anim.OnFire();
        }

        /// <summary>
        /// 朝方向开火
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="offset"></param>
        public virtual void FireInDirection(Vector2 direction, Vector2 offset)
        {
            _factory.CreateBulletInDirection(bulletType, transform.Position2D() + offset, direction, _turretCollider);
            _anim.OnFire();
        }

        public void FireInDirection(Vector2 direction) => FireInDirection(direction, Vector2.zero);
    }

}
