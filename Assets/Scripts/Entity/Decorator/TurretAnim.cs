using System;
using System.Collections;
using System.Collections.Generic;
using Unitilities;
using UnityEngine;

namespace Entity.Decorator
{
    /// <summary>
    /// 炮塔动画: 当玩家被检测到时, 开始瞄准玩家; 否则保持原地
    /// </summary>
    public class TurretAnim : MonoBehaviour
    {
        [SerializeField] private Transform _turretGun;
        private Quaternion _gunStartQuaternion;
        private Transform _mainCharTran;

        private void Awake()
        {
            _gunStartQuaternion = _turretGun.rotation;
        }

        private void Update()
        {
            UpdateCoreDirection(_mainCharTran.Position2D() - transform.Position2D());
        }

        public void OnDetected(MainCharacter mainChar)
        {
            _mainCharTran = mainChar.transform;
        }

        public void OnDetectLost(MainCharacter mainChar)
        {
            _mainCharTran = null;
        }

        private void UpdateCoreDirection(Vector3 dir)
        {
            var look = Quaternion.LookRotation(Vector3.forward, dir);
            _turretGun.rotation = look * _gunStartQuaternion;
        }
    }

}
