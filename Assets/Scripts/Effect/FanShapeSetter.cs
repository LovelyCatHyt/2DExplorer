using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Effect
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FanShapeSetter : MonoBehaviour
    {
        public static int FillAmountProp = Shader.PropertyToID("_FillAmount");

        public float FillAmount
        {
            get => _fillAmount;
            set
            {
                _fillAmount = value;
                _material.SetFloat(FillAmountProp, _fillAmount);
            }
        }

        [SerializeField] private float _fillAmount;
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        //[Conditional("UNITY_EDITOR")]
        //private void OnValidate()
        //{
        //    if(!_material) _material = GetComponent<SpriteRenderer>().material;
        //    _material.hideFlags = HideFlags.DontSave;
        //    _material.SetFloat(FillAmountProp, _fillAmount);
        //}
    }

}
