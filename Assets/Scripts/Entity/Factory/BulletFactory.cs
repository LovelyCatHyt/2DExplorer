using System;
using System.Collections.Generic;
using Unitilities;
using Unitilities.Serialization;
using UnityEngine;

namespace Entity.Factory
{
    /// <summary>
    /// 子弹工厂. 与 Zenject.IFactory 没有关系, 因为 Zenject 的工厂方法不能指定参数.
    /// </summary>
    public class BulletFactory : MonoBehaviour
    {
        public List<SerializableKeyValuePair<string, GameObjectPool>> bulletPoolList;
        private Dictionary<string, GameObjectPool> _bulletPoolDict;

        private void Awake()
        {
            _bulletPoolDict = UnityDictConverter.ConvertToDict(bulletPoolList);
        }

        /// <summary>
        /// 创建一个朝着位置的子弹
        /// </summary>
        /// <param name="bulletType"></param>
        /// <param name="pos"></param>
        /// <param name="target"></param>
        /// <param name="fromCollider"></param>
        /// <returns></returns>
        public GameObject CreateBullet(string bulletType, Vector2 pos, Vector2 target, Collider2D fromCollider)
        {
            if(_bulletPoolDict[bulletType].Pop(out var b))
            {
                var bullet = b.GetComponent<Bullet>();
                if (bullet)
                {
                    bullet.InitWithTarget(pos, target, fromCollider);
                }
                return b;
            }
            else
            {
                throw new NullReferenceException($"BulletPool of {bulletType} is empty!");
            }
        }

        /// <summary>
        /// 创建一个朝着指定方向的子弹
        /// </summary>
        /// <param name="bulletType"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <param name="fromCollider"></param>
        /// <returns></returns>
        public GameObject CreateBulletInDirection(string bulletType, Vector2 pos, Vector2 direction, Collider2D fromCollider)
        {
            if (_bulletPoolDict[bulletType].Pop(out var b))
            {
                var bullet = b.GetComponent<Bullet>();
                if (bullet)
                {
                    bullet.InitWithDirection(pos, direction, fromCollider);
                }
                return b;
            }
            else
            {
                throw new NullReferenceException($"BulletPool of {bulletType} is empty!");
            }
        }
    }

}
