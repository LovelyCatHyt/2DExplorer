using System;
using System.Globalization;
using Map;
using TileDataIO;
using Ui.Prototype;
using UnityEngine;
using Zenject;

namespace Ui
{
    /// <summary>
    /// 地图信息的 Ui
    /// </summary>
    public class MapInfoUi : MonoBehaviour
    {
        [SerializeField] private StringInputField _createdTime;
        [SerializeField] private StringInputField _lastEditedTime;
        [SerializeField] private StringInputField _author;
        [SerializeField] private StringInputField _displayName;

        [Inject] private MapContext _mapContext;
        [Inject] private TileDataMgr _tileDataMgr;
        private MapInfo _tempInfo;

        private void Awake()
        {
            _displayName.onValueChanged.AddListener(OnDisplayNameChanged);
        }

        private void OnDisplayNameChanged(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                _displayName.SetValueNoNotify(displayName);
            }
            else
            {
                var info = _mapContext.Info;
                info.displayName = displayName;
                _mapContext.Info = info;
            }
        }

        public void ShowUp()
        {
            RefreshUi();
        }

        private void RefreshUi()
        {
            var info = _tempInfo = _mapContext.Info;
            _createdTime.Value = info.createdTime.ToString(CultureInfo.CurrentCulture);
            _lastEditedTime.Value = info.lastEditedTime.ToString(CultureInfo.CurrentCulture);
            _author.Value = string.IsNullOrEmpty(info.author) ? "Default" : info.author;
            _displayName.Value = string.IsNullOrEmpty(info.displayName) ? _mapContext.MapFolderName : info.displayName;
        }

        public void Resume()
        {
            _mapContext.Info = _tempInfo;
            RefreshUi();
        }

        public void SaveInfo()
        {
            _tileDataMgr.SaveMapInfo();
        }
    }

}
