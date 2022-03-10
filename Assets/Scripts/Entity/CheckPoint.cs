using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Tiles;
using UnityEngine;
using Zenject;

namespace Entity
{
    /// <summary>
    /// 检查点
    /// </summary>
    public class CheckPoint : MonoBehaviour, IHasExtraData
    {
        public MainCharacter connectedChar;
        public int index;
        public float colorChangeDuration;
        [ColorUsage(true, true)] public Color initColor;
        [ColorUsage(true, true)] public Color activatedColor;
        [ColorUsage(true, true)] public Color disconnectColor;

        /// <summary>
        /// 额外数据
        /// </summary>
        public Dictionary<string, object> ExtraData
        {
            get
            {
                var dict = new Dictionary<string, object> { { "CheckPoint.index", index } };
                return dict;
            }
            set
            {
                if (value == null) return;
                if (!value.TryGetValue("CheckPoint.index", out var i)) return;
                try
                {
                    index = Convert.ToInt32(i);
                }
                catch (InvalidCastException)
                {
                    Debug.LogWarning($"Found an invalid extra data with key \"CheckPoint.index\"! value type is {i.GetType().Name}");
                }
            }
        }

        [Inject] private GameInstance _game;
        [Inject] private TilemapManager _tilemapManager;
        private readonly int _emissionProp = Shader.PropertyToID("_Emission");
        private Material _material;

        private void OnValidate()
        {
            var mat = GetComponent<Renderer>().sharedMaterial;
            mat.SetColor(_emissionProp, initColor);
        }

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        [Inject]
        private void Init(MainCharacter mainCharacter)
        {
            if (index != 0) return;
            connectedChar = mainCharacter;
            connectedChar.currentCheckPoint = this;
            connectedChar.startCheckPoint = this;
        }

        private void Start()
        {
            // 由于在实例化的过程中就会调用 Awake, 无法保证注入成功, 因此在 Start 中才能调用需要被注入的字段
            // _tilemapManager.RecordGameObject(gameObject);
            _material.SetColor(_emissionProp, index == 0 ? activatedColor : initColor);
        }

        private void Disconnect()
        {
            connectedChar = null;
            _material.DOColor(disconnectColor, _emissionProp, colorChangeDuration);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var character = other.gameObject.GetComponent<MainCharacter>();
            if (!character) return;
            if (character.currentCheckPoint.index < index)
            {
                // 断开上一个检查点的连接, 重定位到 this
                character.currentCheckPoint.Disconnect();
                character.currentCheckPoint = this;
                connectedChar = character;
                _material.DOColor(activatedColor, _emissionProp, colorChangeDuration);
            }

            if (character.currentCheckPoint == this)
            {
            }
        }

    }

}
