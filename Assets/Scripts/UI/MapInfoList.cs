using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Map;
using TileDataIO;
using Ui.Prototype;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui
{
    /// <summary>
    /// 对玩家只读的地图信息, 用于显示地图列表
    /// </summary>
    public class MapInfoList : MonoBehaviour
    {
        public GameObject mapListElementPrefab;

        // TODO: [Dev:MapInfoReadOnlyUi] convert to readonly version, and connect to listView
        [SerializeField] private ListView _listView;
        [SerializeField] private Text _createdTime;
        [SerializeField] private Text _lastEditedTime;
        [SerializeField] private Text _author;
        [SerializeField] private Text _displayName;
        [Inject] private TileDataMgr _tileDataMgr;
        [Inject] private MapContext _mapContext;

        private void Awake()
        {
            _listView.onSelectionChanged.AddListener(OnSelectionChanged);
        }

        private void SetInfo(MapInfo info)
        {
            _createdTime.text = info.createdTime.ToString(CultureInfo.CurrentCulture);
            _lastEditedTime.text = info.lastEditedTime.ToString(CultureInfo.CurrentCulture);
            _author.text = string.IsNullOrEmpty(info.author) ? "Null" : info.author;
            _displayName.text = info.displayName;
        }

        private void SetNullInfo()
        {
            _createdTime.text = "--";
            _lastEditedTime.text = "--";
            _author.text = "--";
            _displayName.text = "--";
        }

        private void OnSelectionChanged(ListElement element)
        {
            if (!element)
            {
                SetNullInfo();
                return;
            }

            var mapListElement = element.GetComponent<MapListElement>();
            if (!mapListElement)
            {
                SetNullInfo();
                return;
            }
            SetInfo(mapListElement.Info);
        }

        public void ShowUp()
        {
            var infoAndDirList = _tileDataMgr.MapInfos;
            foreach (var infoAndDir in infoAndDirList)
            {
                var e = _listView.Add(mapListElementPrefab).GetComponent<MapListElement>();
                e.SetInfo(infoAndDir.Item1, infoAndDir.Item2);
            }
        }

        public void Hide()
        {
            _listView.RemoveAll();
        }

        public void LoadCurrent()
        {
            if (!_listView.Selected) return;
            var e = _listView.Selected.GetComponent<MapListElement>();
            if(!e) return;
            _mapContext.OnMapSelected(e.Info, e.MapDir);
            // TODO: Load map
        }
    }

}
