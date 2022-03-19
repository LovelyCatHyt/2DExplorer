using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEditor;
using UnityEngine;

namespace Ui.Prototype
{
    /// <summary>
    /// 场景中的文本
    /// </summary>
    [RequireComponent(typeof(TextMesh))]
    public class SceneText : MonoBehaviour, IHasExtraData
    {
        public Dictionary<string, object> ExtraData
        {
            get => new Dictionary<string, object> { { "Text", Text } };
            set
            {
                if (value.TryGetValue("Text", out var t))
                {
                    Text = (string)t;
                }
            }
        }

        [TextArea] public string text;
        public string Text
        {
            get => _text.text;
            set => text = _text.text = value;
        }

        private TextMesh _text;

        private void Awake()
        {
            _text = GetComponent<TextMesh>();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _text = GetComponent<TextMesh>();
                Text = text;
            }
#endif
        }
    }

}
