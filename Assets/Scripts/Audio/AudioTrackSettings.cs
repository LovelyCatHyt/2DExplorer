using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settings;

namespace Audio
{
    /// <summary>
    /// 音频轨道配置
    /// </summary>
    [CreateAssetMenu(fileName = "New AudioTrack Settings", menuName = "Settings/Audio Track Settings")]
    public class AudioTrackSettings : SettingsObject
    {
        public float Volume
        {
            get => _volume;
            set
            {
                if (Math.Abs(value - _volume) > 0.001f)
                {
                    _volume = value;
                    OnPropertyChanged(nameof(Volume));
                }
            }
        }

        public int MaxAudioSource
        {
            get => _maxAudioSource;
            set
            {
                if (value != _maxAudioSource)
                {
                    _maxAudioSource = value;
                    OnPropertyChanged(nameof(MaxAudioSource));
                }
            }
        }

        [SerializeField] [Range(0, 1)] private float _volume = 1;
        [SerializeField] [Min(1)] private int _maxAudioSource = 1;
    }

}
