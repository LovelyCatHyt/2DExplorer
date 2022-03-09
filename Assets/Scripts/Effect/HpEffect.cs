using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Effect
{
    public class HpEffect : MonoBehaviour
    {
        public FanShapeSetter immediateSetter;
        public FanShapeSetter delayedSetter;
        [Min(0)] public float approachTime;
        public Ease easeMode = Ease.OutSine;

        public float Hp
        {
            set
            {
                _delayedValueTween?.Kill();
                immediateSetter.FillAmount = value;
                _delayedValueTween =
                    DOTween.To(
                        () => delayedSetter.FillAmount, 
                        v => delayedSetter.FillAmount = v, 
                        value, 
                        approachTime)
                        .SetEase(easeMode);
            }
        }

        private Tween _delayedValueTween;

    }

}
