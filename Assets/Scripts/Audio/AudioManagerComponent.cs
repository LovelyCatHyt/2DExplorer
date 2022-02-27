using System;
using System.Collections;
using System.Collections.Generic;
using Unitilities.Serialization;
using UnityEngine;
using Zenject;

namespace Audio
{
    /// <summary>
    /// 音频管理器所在的组件
    /// </summary>
    public class AudioManagerComponent : MonoBehaviour
    {
        public AudioManager audioManager = new AudioManager();
        public GameObject audioTrackPrefab;
        
        [Inject]
        private void OnInject()
        {
            var trackCount = audioManager.TrackCount;
            var trackList = new List<AudioTrack>(trackCount);
            for (int i = 0; i < trackCount; i++)
            {
                var track = Instantiate(audioTrackPrefab, transform).GetComponent<AudioTrack>();
                trackList.Add(track);
            }
            audioManager.Init(trackList);
        }
    }

}
