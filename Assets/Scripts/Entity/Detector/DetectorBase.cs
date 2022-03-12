using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Entity.Detector
{
    /// <summary>
    /// 检测器基类
    /// </summary>
    public class DetectorBase : MonoBehaviour
    {
        public UnityEvent<MainCharacter> onDetected;
        public UnityEvent<MainCharacter> onDetectLost;
    }
}
