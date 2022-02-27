using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// 音频轨道配置
    /// </summary>
    [CreateAssetMenu(fileName ="New AudioTrack Settings", menuName = "Settings/Audio Track Settings")] 
    public class AudioTrackSettings : ScriptableObject
    {
        [Range(0, 1)] public float volume = 1;
        [Min(1)] public int maxAudioSource = 1;
    }

}
