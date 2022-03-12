using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Entity.Detector;
using Unitilities;
using UnityEngine;

namespace Entity.Decorator
{
    /// <summary>
    /// 炮塔动画: 当玩家被检测到时, 开始瞄准玩家; 否则保持原地
    /// </summary>
    public class TurretAnim : MonoBehaviour, IDetectHandler
    {
        public bool autoAimTarget = true;
        [Min(0)] public float fireEffectDuration = 0.2f;
        [ColorUsage(true, true)] public Color fireColor;
        
        [SerializeField] private Transform _turretGun;
        [SerializeField] private Renderer _renderer;
        private Material _material;
        private Quaternion _gunStartQuaternion;
        private Transform _mainCharTran;

        private void Awake()
        {
            _gunStartQuaternion = _turretGun.rotation;
            if (!_renderer) _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
        }

        private void Update()
        {
            if(_mainCharTran && autoAimTarget) UpdateDirection(_mainCharTran.Position2D() - transform.Position2D());
        }

        public void OnDetected(IRole target)
        {
            _mainCharTran = target.RoleTransform;
        }

        public void OnDetectLost(IRole target)
        {
            _mainCharTran = null;
        }

        public void UpdateDirection(Vector3 dir)
        {
            var look = Quaternion.LookRotation(Vector3.forward, dir);
            _turretGun.rotation = look * _gunStartQuaternion;
        }

        public void OnFire()
        {
            _material.DOKill(true);
            _material.DOColor(fireColor, fireEffectDuration).SetLoops(2, LoopType.Yoyo);
        }
    }

}
