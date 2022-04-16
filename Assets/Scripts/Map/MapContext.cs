using System;
using System.IO;
using Newtonsoft.Json;
using TileDataIO;

namespace Map
{
    /// <summary>
    /// 地图上下文, 可以管理当前地图的信息
    /// </summary>
    public class MapContext
    {
        public MapInfo Info
        {
            get => _info!.Value;
            set => _info = value;
        }
        /// <summary>
        /// 当前地图根目录
        /// </summary>
        public string MapDir { get; private set; } = "";
        /// <summary>
        /// 地图文件夹名字, 是地图目录中最后一个文件夹的名字
        /// </summary>
        public string MapFolderName
        {
            get
            {
                var index = MapDir.LastIndexOf(Path.DirectorySeparatorChar);
                return MapDir.Substring(index + 1);
            }
        }
        public string TileDataDirectory => Path.Combine(MapDir, _info!.Value.tileDataDirectory);
        public string EntityDataPath => Path.Combine(MapDir, _info!.Value.entityDataPath);
        /// <summary>
        /// 当前地图为有效的地图
        /// </summary>
        public bool MapAvailable { get; private set; } = false;
        /// <summary>
        /// 当前地图已经被修改
        /// </summary>
        public bool Dirty
        {
            get => _dirty;
            set
            {
                if (_dirty != value)
                {
                    OnDirtyChanged?.Invoke(_dirty = value);
                }
            }
        }
        
        public delegate void MapDirtyEvent(bool isDirtyNow);

        public event MapDirtyEvent OnDirtyChanged;

        private MapInfo? _info = null;
        /// <summary>
        /// 当前地图已经被修改
        /// </summary>
        private bool _dirty = false;
        
        /// <summary>
        /// 地图选中但未加载
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dir"></param>
        public void OnMapSelected(MapInfo target, string dir)
        {
            _info = target;
            MapDir = dir;
            _dirty = false;
            MapAvailable = false;
        }

        /// <summary>
        /// 地图载入完成
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dir"></param>
        public void OnMapLoaded(MapInfo info, string dir)
        {
            this._info = info;
            MapDir = Path.GetFullPath(dir);
            _dirty = false;
            MapAvailable = true;
        }

        /// <summary>
        /// 地图关闭: 返回主菜单或其它导致场景切换的情形; 退出游戏
        /// </summary>
        public void OnMapClosed()
        {
            _info = null;
            MapDir = "";
            _dirty = false;
            MapAvailable = false;
        }
    }

}
