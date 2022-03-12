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
            var mainChar = other.GetComponent<MainCharacter>();
            if (mainChar)
            {
                onDetected?.Invoke(mainChar);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var mainChar = other.GetComponent<MainCharacter>();
            if (mainChar)
            {
                onDetectLost?.Invoke(mainChar);
            }
        }
    }

}
