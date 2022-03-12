using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Entity.Detector
{
    public interface IDetectHandler
    {
        public void OnDetected(IRole role);
        public void OnDetectLost(IRole role);
    }

    /// <summary>
    /// 检测器基类
    /// </summary>
    public class DetectorBase : MonoBehaviour
    {
        public UnityEvent onDetected;
        public UnityEvent onDetectLost;
        public virtual IRole DetectedRole
        {
            get;
            protected set;
        }

        private bool _initialized = false;

        private void Awake()
        {
            Init();
        }

        [Inject]
        public void Init()
        {
            if(_initialized) return;
            foreach (var handler in GetComponentsInChildren<IDetectHandler>())
            {
                onDetected.AddListener(() => handler.OnDetected(DetectedRole));
                onDetectLost.AddListener(() => handler.OnDetectLost(DetectedRole));
            }
            _initialized = true;
        }
    }
}
