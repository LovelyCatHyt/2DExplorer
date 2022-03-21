using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Audio;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Settings
{
    /// <summary>
    /// 设置管理器
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SettingsManager
    {

        public const string SettingsFilePathRelative
#if UNITY_EDITOR
            = "../Config/Settings.json";
#else
            = "Config/Settings.json";
#endif
        public static string SettingsFullPath => Path.Combine(Application.dataPath, SettingsFilePathRelative);
        public const Formatting OutputFormat
#if UNITY_EDITOR
            = Formatting.Indented;
#else
            = Formatting.None;
#endif

        [JsonProperty("audio")]
        private Dictionary<string, AudioTrackSettings> _audioTrackSettings;
        private AudioManager _audioManager;
        
        public void AfterSceneLoaded(AudioManager audioManager)
        {
            _audioManager = audioManager;
            LoadSettings();
        }

        public void SaveSettings()
        {
            _audioTrackSettings = _audioManager.SettingsDict;
            // 直接把自己塞进去
            var jsonText = JsonConvert.SerializeObject(this, OutputFormat);
            var fullPath = SettingsFullPath;
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            File.WriteAllText(fullPath, jsonText);
        }

        public void LoadSettings()
        {
            try
            {
                JsonConvert.PopulateObject(File.ReadAllText(SettingsFullPath), this);
            }
            catch (Exception e)when (e is FileNotFoundException || e is DirectoryNotFoundException)
            {
                Debug.Log("Settings file not found. Create default settings files.");
                SaveSettings();
            }
            catch (JsonReaderException e)
            {
                Debug.Log($"Settings file format not correct. See the exception below:\n{e}");
                File.Move(SettingsFullPath, $"{SettingsFullPath}.BadFormat");
                Debug.Log($"Move incorrect settings file to \"{SettingsFullPath}.BadFormat\"");
                SaveSettings();
            }
        }

    }

}
