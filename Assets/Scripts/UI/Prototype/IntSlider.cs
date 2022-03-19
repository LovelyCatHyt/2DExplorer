using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui.Prototype
{
    /// <summary>
    /// 整型滑块
    /// </summary>
    public class IntSlider : MonoBehaviour
    {
        public UnityEvent<int> onValueChanged;
        public int Value
        {
            get => (int) _slider.value;
            set => _slider.value = value;
        }

        [SerializeField] private Slider _slider;
        [SerializeField] private InputField _inputField;

        private void Awake()
        {
            if (!_slider) _slider = GetComponent<Slider>();
            if (!_inputField) _inputField = GetComponent<InputField>();
            _slider.onValueChanged.AddListener(OnSliderChanged);
            _inputField.onEndEdit.AddListener(OnFieldChanged);
            onValueChanged.AddListener(value =>
            {
                value = Mathf.Clamp(value, (int)_slider.minValue, (int)_slider.maxValue);
                _slider.SetValueWithoutNotify(value);
                _inputField.SetTextWithoutNotify(value.ToString());
            });
            onValueChanged.Invoke((int) _slider.value);
        }

        private void OnFieldChanged(string arg0)
        {
            if (!int.TryParse(arg0, out var value))
            {
                value = (int) _slider.value;
            }
            onValueChanged?.Invoke(value);
        }

        private void OnSliderChanged(float arg0)
        {
            onValueChanged?.Invoke((int)arg0);
        }
    }

}
