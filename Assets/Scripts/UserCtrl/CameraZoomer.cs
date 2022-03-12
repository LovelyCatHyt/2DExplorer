using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserCtrl
{
    /// <summary>
    /// 摄像机缩放
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraZoomer : MonoBehaviour
    {
        public float maxSize = 16;
        public float minSize = 5;
        public float scrollScale = 1;

        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Update()
        {
            var amount = Input.GetAxis("Mouse ScrollWheel") * scrollScale;
            var temp = Mathf.Min(maxSize, Mathf.Max(minSize, _cam.orthographicSize - amount));
            _cam.orthographicSize = temp;
        }
    }

}
