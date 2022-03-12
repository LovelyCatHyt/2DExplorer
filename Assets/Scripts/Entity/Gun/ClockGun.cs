using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Entity.Decorator;
using Entity.Detector;
using Unitilities;
using UnityEngine;

namespace Entity.Gun
{
    [AddComponentMenu("GameMain/Gun/ClockGun")]
    public class ClockGun : GunBase, IDetectHandler
    {
        [Flags]
        public enum TimerBehaviour
        {
            /// <summary>
            /// 持续计时, 此项为 0 则仅当检测到目标时计时
            /// </summary>
            AlwaysCount = 1 << 0,
            /// <summary>
            /// 检测丢失时重置计时
            /// </summary>
            ResetWhenDetectLost = 1 << 1
        }

        [Delayed] [Min(0)] public float interval;
        public TimerBehaviour timerBehaviour;
        public bool randomInitTime;
        public float minAimingTime = 0.5f;
        
        private const float perlinNoiseScale = .05f;
        private IRole _target;
        private float _fireTimer = 0;
        private Vector2 _currentDirection;
        private Vector2 CurrentDirection
        {
            get => _currentDirection;
            set
            {
                _anim.UpdateDirection(value);
                _currentDirection = value;
            }
        }
        private Tween _aimingTween;

        public void OnDetected(IRole target)
        {
            _target = target;
        }

        public void OnDetectLost(IRole mainChar)
        {
            _target = null;
        }

        private void Start()
        {
            if (randomInitTime)
            {
                var scaledPos = transform.Position2D() * perlinNoiseScale;
                _fireTimer = Mathf.PerlinNoise(scaledPos.x, scaledPos.y) * interval;
            }
        }

        private void FixedUpdate()
        {
            // 更新瞄准方向
            if (_target != null)
            {
                SetAimDirection(_target.RoleTransform.Position2D() - transform.Position2D());
            }
            else
            {
                // SetAimDirection(Vector2.up);
            }

            // 持续计时或仅检测到目标时计时
            if ((timerBehaviour & TimerBehaviour.AlwaysCount) != 0 || _target != null)
            {
                _fireTimer += Time.fixedDeltaTime;
            }
            // 如果启用 ResetWhenDetectLost 且目标为空, 则重置计时
            if ((timerBehaviour & TimerBehaviour.ResetWhenDetectLost) != 0)
            {
                if (_target == null) _fireTimer = 0;
            }
            // 达到发射周期
            if (_fireTimer > interval && _target != null)
            {
                FireInDirection(CurrentDirection);
            }

            _fireTimer %= interval;
        }

        private void SetAimDirection(Vector2 direction)
        {
            _aimingTween?.Kill();
            _aimingTween = DOTween.To(() => CurrentDirection, value => CurrentDirection = value, direction,
                minAimingTime);
        }
    }

}
