using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.Detector
{
    /// <summary>
    /// OnTrigger 时触发
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TriggerDetector : DetectorBase
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponent<IRole>();
            if (target != null)
            {
                DetectedRole = target;
                onDetected?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var target = other.GetComponent<MainCharacter>();
            if (target != null)
            {
                DetectedRole = null;
                onDetectLost?.Invoke();
            }
        }
    }

}
