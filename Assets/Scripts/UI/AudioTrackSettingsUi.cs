using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Ui.Prototype;
using UnityEngine;

namespace Ui
{
    /// <summary>
    /// 音轨设置 UI
    /// </summary>
    public class AudioTrackSettingsUi : MonoBehaviour
    {
        public AudioTrackSettings target;

        [SerializeField] private IntSlider _volume;
        [SerializeField] IntSlider _maxSourceCount;

        private void Awake()
        {
            _volume.Value = (int)(target.Volume * 100);
            _volume.onValueChanged.AddListener(value => target.Volume = value/100f);
            _maxSourceCount.Value = target.MaxAudioSource;
            _maxSourceCount.onValueChanged.AddListener(value=>target.MaxAudioSource = value);
        }
    }

}
