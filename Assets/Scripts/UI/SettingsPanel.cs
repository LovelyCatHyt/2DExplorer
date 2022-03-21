using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Settings;
using UnityEngine;
using Zenject;

namespace Ui
{
    /// <summary>
    /// 设置面板
    /// </summary>
    public class SettingsPanel : MonoBehaviour
    {
        [Inject] private SettingsManager _settingsManager;

        [UsedImplicitly]
        public void OnQuitSettings()
        {
            _settingsManager.SaveSettings();
        }
    }

}
