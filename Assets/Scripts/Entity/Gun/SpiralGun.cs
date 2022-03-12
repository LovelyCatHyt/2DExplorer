using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Detector;
using UnityEngine;

namespace Entity.Gun
{
    /// <summary>
    /// 螺旋枪, 持续发射, 超强火力
    /// </summary>
    public class SpiralGun : GunBase, IDetectHandler
    {
        public float timeInterval = .1f;
        public float angleStep = 14;

        private float _timer;
        private float _currentAngle;
        private IRole _target;
        
        // TODO: get real bulletSpeed
        private float _bulletSpeed = 5f;

        public void OnDetected(IRole role)
        {
            _target = role;
        }

        public void OnDetectLost(IRole role)
        {
            _target = null;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;
            _timer += Time.fixedDeltaTime;

            var skippedFrames = (int)(_timer / timeInterval);
            // Debug.Log($"SkippedFrames:{skippedFrames}");
            float offsetDistance = skippedFrames * timeInterval * _bulletSpeed;
            while (_timer >= timeInterval)
            {
                // 每隔一段时间发射一次
                // 一帧里可以发射好多个(doge
                var start = Vector3.down;
                var rot = Quaternion.Euler(0, 0, _currentAngle);
                var direction = rot * start;
                FireInDirection(direction, direction * offsetDistance);
                _currentAngle += angleStep;
                _currentAngle = Mathf.Repeat(_currentAngle, 360);
                _timer -= timeInterval;
                offsetDistance -= timeInterval * _bulletSpeed;
            }
        }
    }

}
