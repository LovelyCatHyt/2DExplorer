using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unitilities;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

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
        [ColorUsage(true, true)] public Color endColor;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration;
        /// <summary>
        /// 爆炸特效结束后的回调
        /// </summary>
        public UnityEvent onFinish;
        [Inject]
        private GameObjectPool _explosionPool;

        private Material _material_s => _material ??= GetComponent<Renderer>().material;
        private Material _material;

        private void OnEnable()
        {
            var mat = _material_s;
            mat.color = startColor;
            mat.DOColor(endColor, duration).OnComplete(() =>
            {
                onFinish.Invoke();
                _explosionPool.Push(gameObject);
            });
        }
    }

}
