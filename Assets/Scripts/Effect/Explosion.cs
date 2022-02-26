using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Effect
{
    /// <summary>
    /// 爆炸效果
    /// </summary>
    public class Explosion : MonoBehaviour
    {
        /// <summary>
        /// 起始颜色
        /// </summary>
        [ColorUsage(true, true)] public Color startColor;
        /// <summary>
        /// 结束颜色
        /// </summary>
        [ColorUsage(true, true)]  public Color endColor;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration;
        /// <summary>
        /// 自动销毁
        /// </summary>
        public bool autoDestroy = true;
        /// <summary>
        /// 爆炸特效结束后的回调
        /// </summary>
        public UnityEvent onFinish;

        private void Awake()
        {
            Material material = GetComponent<Renderer>().material;
            material.color = startColor;
            material.DOColor(endColor, duration).OnComplete(() =>
            {
                onFinish.Invoke();
                if(autoDestroy) Destroy(gameObject);
            });
        }
    }

}
