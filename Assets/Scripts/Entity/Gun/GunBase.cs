using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            if (!_turretCollider)
            {
                _turretCollider = GetComponent<Collider2D>();
            }
        }

        /// <summary>
        /// 开火
        /// </summary>
        /// <param name="targetTran">目标的 <see cref="Transform"/></param>
        public virtual void Fire(Transform targetTran) => Fire(targetTran.position);

        /// <summary>
        /// 开火
        /// </summary>
        /// <param name="target">目标位置</param>
        public virtual void Fire(Vector2 target)
        {
            _factory.CreateBullet(bulletType, transform.Position2D(), target, _turretCollider);
        }
    }

}
