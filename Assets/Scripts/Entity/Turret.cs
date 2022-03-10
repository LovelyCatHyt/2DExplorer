using System;
using Audio;
using DG.Tweening;
using DI;
using Tiles;
using Unitilities;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Entity
{
    /// <summary>
    /// 炮塔
    /// </summary>
    public class Turret : MonoBehaviour, ITileGOComponent
    {

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
        public Renderer effectRenderer;
        public float fireEffectDuration;
        [ColorUsage(true, true)] public Color fireColor;
        public AudioClip fireSound;

        /// <summary>
        /// 子弹对象池
        /// </summary>
        [Inject(Id = "Bullet Pool")]
        private GameObjectPool _bulletPool;
        [Inject]
        private AudioManager _audioManager;
        /// <summary>
        /// 炮塔核心
        /// </summary>
        [SerializeField] 
        private Transform turretCore;
        private Collider2D _collider;
        private Quaternion _coreStartQuaternion;
        /// <summary>
        /// 射击计时器
        /// </summary>
        private float _fireTimer;
        private Vector3Int _position;
        [Inject]
        private MainCharacter _mainChar;
        /// <summary>
        /// 射线检测缓存
        /// </summary>
        private readonly RaycastHit2D[] _raycastHitCache = new RaycastHit2D[2];
        private Material _material;
        /// <summary>
        /// 用于随机初始相位的柏林噪声的尺度比例
        /// </summary>
        private const float NoiseScale = .1f;

        /// <summary>
        /// 启动时记录相应的坐标和子弹对象池
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        public virtual void StartUp(Vector3Int position, Tilemap tilemap)
        {
            gameObject.name = $"Turret {position}";
            _position = position;
            _fireTimer = Mathf.PerlinNoise(position.x * NoiseScale, position.y * NoiseScale) * fireInterval;
        }
        
        private void Awake()
        {
            _coreStartQuaternion = turretCore.rotation;
            _collider = GetComponentInChildren<Collider2D>();
            _material = effectRenderer.material;
        }

        private void Start()
        {
            // 注入一下
            // 不在 Awake 注入的原因是, Tilemap 使用 Instantiate 时, 尽管 Awake 会被调用, 但本物体的上级物体是空的(null).
            GetComponentInParent<ContainerEntry>().Container.Inject(this);
        }

        private void FixedUpdate()
        {
            _fireTimer += Time.fixedDeltaTime;
            UpdateCoreDirection(_mainChar.transform.position - transform.position);
            if (_fireTimer >= fireInterval)
            {
                var target = _mainChar.transform.Position2D();
                bool hitTriggers = Physics2D.queriesHitTriggers;
                Physics2D.queriesHitTriggers = false;
                var hitCount = Physics2D.RaycastNonAlloc(transform.Position2D(), target - transform.Position2D(),
                    _raycastHitCache, detectRange);
                Physics2D.queriesHitTriggers = hitTriggers;
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

            if (!_bulletPool.Pop(out var bulletGO))
            {
                Debug.LogError($"{_bulletPool.name} can't pop bullet out. check the pool settings.");
                return;
            }

            var bullet = bulletGO.GetComponent<Bullet>();
            var position = transform.position;
            bullet.Init(position + (target - position).normalized * fireStartDistance, target, _collider);
            _material.DOKill(true);
            _material.DOColor(fireColor, fireEffectDuration * 0.5f).SetLoops(2, LoopType.Yoyo);
            _audioManager.PlaySound(_audioManager.GetTrackIDFromName("Sound"), fireSound, position);
        }
    }
}
