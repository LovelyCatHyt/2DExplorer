using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unitilities.Serialization;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    [Serializable]
    public class AudioManager
    {
        
        public List<SerializableKeyValuePair<string, AudioTrackSettings>> settingsMap_List = new List<SerializableKeyValuePair<string, AudioTrackSettings>>();
        public int TrackCount => settingsMap_List.Count;
        public Dictionary<string, AudioTrackSettings> SettingsDict=>new Dictionary<string, AudioTrackSettings>(_settingsMap);

        private List<AudioTrackSettings> _settings;
        private Dictionary<string, AudioTrackSettings> _settingsMap;
        private List<AudioTrack> _trackList;
        private Dictionary<string, int> _trackIdDictionary;

        /// <summary>
        /// 初始化音频管理器
        /// </summary>
        /// <param name="tracks"></param>
        public void Init(IEnumerable<AudioTrack> tracks)
        {
            _trackList = tracks.ToList();
            _settingsMap = UnityDictConverter.ConvertToDict(settingsMap_List);
            _settings = new List<AudioTrackSettings>();

            // 建立映射表 并应用音轨设置
            _trackIdDictionary = new Dictionary<string, int>();
            using (var keys = _settingsMap.Keys.GetEnumerator())
            {
                for (var i = 0; keys.MoveNext(); i++)
                {
                    var key = keys.Current ?? "";
                    _trackIdDictionary[key] = i;
                    _settings.Add(_settingsMap[key]);
                    _settings[i].PropertyChanged += _trackList[i].OnSettingsChanged;
                    _trackList[i].ApplySettings(_settings[i]);
                    _trackList[i].gameObject.name = key;
                }
            }
        }

        public int GetTrackIDFromName(string name) => _trackIdDictionary[name];

        public void PlaySound(int trackId, AudioClip clip, bool loop = false)
        {
            if (trackId < 0 || trackId >= _trackList.Count) throw new ArgumentException("Track ID exceeded!");
            _trackList[trackId].PlaySound(clip, loop);
        }

        public void PlaySound(int trackId, AudioClip clip, Vector3 position, bool loop = false)
        {
            if (trackId < 0 || trackId >= _trackList.Count) throw new ArgumentException("Track ID exceeded!");
            _trackList[trackId].PlaySound(clip, position, loop);
        }
    }
}
