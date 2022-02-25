using System;
using System.Linq;
using CharCtrl;
using Tiles;
using Unitilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Entity
{
    /// <summary>
    /// 炮塔
    /// </summary>
    public class Turret : MonoBehaviour, ITileGOComponent
    {
        /// <summary>
        /// 子弹对象池
        /// </summary>
        public GameObjectPool bulletPool;
        /// <summary>
        /// 开火间隔
        /// </summary>
        public float fireInterval;
        /// <summary>
        /// 检测范围
        /// </summary>
        public float detectRange;
        /// <summary>
        /// 发射距离
        /// </summary>
        public float fireStartDistance = 1.5f;

        /// <summary>
        /// 炮塔核心
        /// </summary>
        [SerializeField] private Transform turretCore;
        private Quaternion _coreStartQuaternion;
        /// <summary>
        /// 射击计时器
        /// </summary>
        private float _fireTimer;
        private Vector3Int _position;
        private MainCharacter _mainChar;
        /// <summary>
        /// 射线检测缓存
        /// </summary>
        private readonly RaycastHit2D[] _raycastHitCache = new RaycastHit2D[2];

        /// <summary>
        /// 启动时记录相应的坐标和子弹对象池
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        public virtual void StartUp(Vector3Int position, Tilemap tilemap)
        {
            gameObject.name = $"Turret {position}";
            _position = position;
            bulletPool = tilemap.GetComponent<GameObjectPool>();
            _mainChar = FindObjectOfType<MainCharacter>();
            _fireTimer = Mathf.PerlinNoise(position.x, position.y) * fireInterval;
        }

        private void Awake()
        {
            _coreStartQuaternion = turretCore.rotation;
        }

        private void FixedUpdate()
        {
            _fireTimer += Time.fixedDeltaTime;
            UpdateCoreDirection(_mainChar.transform.position - transform.position);
            if (_fireTimer >= fireInterval)
            {
                var target = _mainChar.transform.Position2D();
                var hitCount = Physics2D.RaycastNonAlloc(transform.Position2D(), target - transform.Position2D(),
                    _raycastHitCache, detectRange);
                if (hitCount > 1)
                {
                    // 第一个必定是本身, 即所在的 Tilemap 或者对应的 Sprite. 所以读取第二个是否为玩家
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    if (_raycastHitCache[1].collider.GetComponent<MainCharacter>() == _mainChar)
                    {
                        Fire(target);
                    }
                }

                _fireTimer %= fireInterval;
            }
        }

        private void UpdateCoreDirection(Vector3 dir)
        {
            var look = Quaternion.LookRotation(Vector3.forward, dir);
            turretCore.rotation = look * _coreStartQuaternion;
        }

        /// <summary>
        /// 射击
        /// </summary>
        /// <param name="target"></param>
        public void Fire(Vector3 target)
        {
            // Debug.Log($"{gameObject.name} try to fire.");

            if (!bulletPool.Pop(out var bulletGO))
            {
                Debug.LogError($"{bulletPool.name} can't pop bullet out. check the pool settings.");
                return;
            }

            var bullet = bulletGO.GetComponent<Bullet>();
            bullet.onHit += () => bulletPool.Push(bullet.gameObject);
            bullet.Init(transform.position + (target - transform.position).normalized * fireStartDistance, target);
        }
    }
}
