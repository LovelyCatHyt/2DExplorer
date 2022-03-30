using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui.Prototype
{
    /// <summary>
    /// 字符串输入框
    /// <para>目前其实就是在 <see cref="InputField"/> 的基础上套了一层简陋的封装, 但说不定未来会需要其他的功能.</para>
    /// </summary>
    public class StringInputField : MonoBehaviour
    {
        public UnityEvent<string> onValueChanged;

        public string Value
        {
            get => _inputField.text;
            set => _inputField.text = value;
        }
        
        [SerializeField] private InputField _inputField;

        private void Awake()
        {
            if (!_inputField) _inputField = GetComponentInChildren<InputField>();
            _inputField.onValueChanged.AddListener(OnFieldChanged);
        }

        private void OnFieldChanged(string value)
        {
            SetValueNoNotify(value);
            onValueChanged?.Invoke(value);
        }

        public void SetValueNoNotify(string value) => _inputField.SetTextWithoutNotify(value);
    }

}
