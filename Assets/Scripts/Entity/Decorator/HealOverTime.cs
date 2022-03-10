using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Decorator
{
    /// <summary>
    /// 持续治疗点
    /// </summary>
    public class HealOverTime : MonoBehaviour
    {
        [Tooltip("每秒治疗量")]
        public double healPerSec;
        [Tooltip("按百分比治疗")]
        public bool healByPercent = true;

        private MainCharacter _healTarget;

        private void FixedUpdate()
        {
            if (!_healTarget) return;
            var heal = healPerSec * Time.fixedDeltaTime;
            if (healByPercent)
            {
                _healTarget.HealByPercent(heal);
            }
            else
            {
                _healTarget.HealDirectly(heal);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var mainCharacter = other.GetComponent<MainCharacter>();
            if(mainCharacter)
            {
                _healTarget = mainCharacter;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var mainCharacter = other.GetComponent<MainCharacter>();
            if (mainCharacter)
            {
                _healTarget = null;
            }
        }
    }

}
