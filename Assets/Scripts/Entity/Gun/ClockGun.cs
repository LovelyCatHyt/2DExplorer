using System;
using System.Collections;
using System.Collections.Generic;
using Unitilities;
using UnityEngine;

namespace Entity.Gun
{
    [AddComponentMenu("GameMain/Gun/ClockGun")]
    public class ClockGun : GunBase
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

        [Min(0)] public float interval;
        public TimerBehaviour timerBehaviour;
        public bool randomInitTime;

        private const float perlinNoiseScale = .05f;
        private MainCharacter _target;
        private float _fireTimer;

        public void OnDetected(MainCharacter mainChar)
        {
            _target = mainChar;
        }

        public void OnDetectLost(MainCharacter mainChar)
        {
            _target = null;
        }

        private void Start()
        {
            if(randomInitTime)
            {
                var scaledPos = transform.Position2D() * perlinNoiseScale;
                Mathf.PerlinNoise(scaledPos.x, scaledPos.y);
            }
        }

        private void FixedUpdate()
        {
            if ((timerBehaviour & TimerBehaviour.AlwaysCount) != 0)
            {
                _fireTimer += Time.fixedDeltaTime;
            }

            if ((timerBehaviour & TimerBehaviour.ResetWhenDetectLost) != 0)
            {
                if (!_target) _fireTimer = 0;
            }

            if (_fireTimer > interval)
            {
                Fire(_target.transform);
            }

            _fireTimer %= interval;
        }
    }

}
