using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// 音轨
    /// </summary>
    public class AudioTrack : MonoBehaviour
    {
        public GameObject audioSourcePrefab;

        private List<AudioSource> _activeList = new List<AudioSource>();
        private List<AudioSource> _inActiveList = new List<AudioSource>();
        private List<AudioSource> _audios = new List<AudioSource>();
        /// <summary>
        /// 音轨下所有音源的音量
        /// </summary>
        private float Volume
        {
            get => _volume;
            set
            {
                foreach (var audioSource in _audios)
                {
                    audioSource.volume = value;
                }
                _volume = value;
            }
        }
        private float _volume;

        /// <summary>
        /// 最大的音源数目
        /// </summary>
        private int MaxAudioSource
        {
            get => _maxAudioSource;
            set
            {
                if (value == _maxAudioSource) return;
                if (value > _maxAudioSource)
                {
                    // 增多
                    for (var i = _maxAudioSource; i < value; i++)
                    {
                        CreateNewAudioSource();
                    }
                }
                else
                {
                    // 减少
                    for (var i = _maxAudioSource; i > value; i--)
                    {
                        RemoveOneAudioSource();
                    }
                }

                _maxAudioSource = value;
            }
        }
        private int _maxAudioSource;
        private AudioTrackSettings _settings;

        private AudioSource CreateNewAudioSource()
        {
            AudioSource audioSource;
            if (audioSourcePrefab)
            {
                // 从 prefab 实例化
                var go = Instantiate(audioSourcePrefab, transform);
                go.name = $"AudioSource[{_audios.Count}]";
                audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = _volume;
                _audios.Add(audioSource);
            }
            else
            {
                // 直接新建物体, 挂在本物体下
                var go = new GameObject($"AudioSource[{_audios.Count}]");
                go.transform.parent = transform;
                audioSource = go.AddComponent<AudioSource>();
                audioSource.volume = _volume;
                _audios.Add(audioSource);
            }
            _inActiveList.Add(audioSource);
            return audioSource;
        }

        /// <summary>
        /// 移除一个音源
        /// </summary>
        private void RemoveOneAudioSource()
        {
            AudioSource audioSource;
            if (_inActiveList.Any())
            {
                audioSource = _inActiveList.Last();
                _inActiveList.Remove(audioSource);
            }
            else
            {
                audioSource = _activeList[0];
                _activeList.Remove(audioSource);
            }
            Destroy(audioSource.gameObject);
            _audios.Remove(audioSource);
        }
        
        private void Update()
        {
            // 遍历两个列表, 将 _activeList 中播放完毕的放到 _inActiveList
            for (var i = 0; i < _activeList.Count; i++)
            {
                if (!_activeList[i].isPlaying)
                {
                    _inActiveList.Add(_activeList[i]);
                    _activeList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void OnSettingsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (sender is AudioTrackSettings settings)
            {
                _settings = settings;
                if (args.PropertyName == nameof(AudioTrackSettings.Volume))
                {
                    Volume = settings.Volume;
                }

                if (args.PropertyName == nameof(AudioTrackSettings.MaxAudioSource))
                {
                    MaxAudioSource = settings.MaxAudioSource;
                }
            }
        }

        public void ApplySettings(AudioTrackSettings settings)
        {
            Volume = settings.Volume;
            MaxAudioSource = settings.MaxAudioSource;
        }

        private void OnDisable()
        {
            if (_settings) _settings.PropertyChanged -= OnSettingsChanged;
        }

        public void PlaySound(AudioClip clip, bool loop = false)
        {
            var target = GetAvailableAudioSource();
            if (!target) return;

            target.spatialBlend = 0;

            if (loop)
            {
                target.Play();
            }
            else
            {
                target.PlayOneShot(clip);
            }
        }

        public void PlaySound(AudioClip clip, Vector3 position, bool loop = false)
        {
            var target = GetAvailableAudioSource();
            if (!target) return;

            target.spatialBlend = 1;
            target.transform.position = position;

            if (loop)
            {
                target.Play();
            }
            else
            {
                target.PlayOneShot(clip);
            }
        }

        private AudioSource GetAvailableAudioSource()
        {
            if (!_audios.Any())
            {
                Debug.LogError($"No audioSource available! in {name}!", this);
                return null;
            }
            AudioSource target;
            if (_inActiveList.Any())
            {
                target = _inActiveList.Last();
                _inActiveList.RemoveAt(_inActiveList.Count - 1);
                _activeList.Add(target);
            }
            else
            {
                // TODO: 一个更合理的复用活跃声源的算法
                target = _activeList[0];
                target.Stop();
            }

            return target;
        }
    }

}
