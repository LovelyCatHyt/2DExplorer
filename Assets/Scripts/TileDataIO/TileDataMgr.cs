using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game;
using Map;
using Newtonsoft.Json;
using Tiles;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace TileDataIO
{
    /// <summary>
    /// 瓦片地图数据管理器
    /// </summary>
    public class TileDataMgr : MonoBehaviour
    {
        /// <summary>
        /// 瓦片资源列表
        /// </summary>
        public ObjectRefTable tileTable;
        /// <summary>
        /// 实体预制体列表
        /// </summary>
        public ObjectRefTable entityTable;
        public Formatting jsonFormat;
        
        public const string MapRootRelative
#if UNITY_EDITOR
            = "../Maps/";
#else
            = "/Maps/"
#endif

        /// <summary>
        /// 默认初始地图目录
        /// </summary>
        public static string DefaultMapDir => Path.Combine(Application.dataPath, MapRootRelative, "Default");

        [Inject] private TilemapManager _tilemapManager;
        [Inject] private MapContext _mapContext;
        [Inject] private GameInstance _game;
        
        private void Awake()
        {
            if (_mapContext.MapAvailable) return;
            // 如果地图上下文没有可用的地图, 就加载第一个地图, 或者保存现有的(通常是在开发时创建的Scene中的地图)
            if (GetFirstMapInfo(out var info, out var directory))
            {
                _mapContext.OnMapLoaded(info, directory);
            }
            else
            {
                // 原地保存一个
                Directory.CreateDirectory(DefaultMapDir);
                var newInfo = MapInfo.CreateInfo();
                _mapContext.OnMapSelected(newInfo, DefaultMapDir);
                // print(DefaultMapDir);
                SaveWholeGrid();
                Debug.Log($"No map available. save current map into <b>{Path.GetFullPath(DefaultMapDir)}</b>");
                _mapContext.OnMapLoaded(newInfo, DefaultMapDir);
            }
        }

        public bool GetFirstMapInfo(out MapInfo info, out string directory)
        {

            foreach (var dir in MapDirectories)
            {
                string infoJson;
                try
                {
                    infoJson = File.ReadAllText(Path.Combine(dir, "Info.json"));
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
                try
                {
                    info = JsonConvert.DeserializeObject<MapInfo>(infoJson);
                    // 不出问题
                    directory = dir;
                    return true;
                }
                catch (JsonReaderException)
                {
                    continue;
                }
            }
            info = MapInfo.CreateInfo();
            directory = "";
            return false;
        }

        public List<MapInfo> MapInfos
        {
            get
            {
                var infoList = new List<MapInfo>();
                foreach (var directory in MapDirectories)
                {
                    string infoJson;
                    try
                    {
                        infoJson = File.ReadAllText(Path.Combine(directory, "Info.json"));
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }

                    MapInfo info;
                    try
                    {
                        info = JsonConvert.DeserializeObject<MapInfo>(infoJson);
                    }
                    catch (JsonReaderException)
                    {
                        continue;
                    }
                    infoList.Add(info);
                }
                return infoList;
            }
        }

        /// <summary>
        /// 从地图目录出发能找到的所有子目录的完整路径, 但不保证存在有效地图
        /// </summary>
        private string[] MapDirectories
        {
            get
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, MapRootRelative));
                return Directory.GetDirectories(Path.Combine(Application.dataPath, MapRootRelative));
            }
        }

        public void SaveMapInfo()
        {
            var json = JsonConvert.SerializeObject(_mapContext.Info, jsonFormat);
            var path = Path.Combine(_mapContext.MapDir, "Info.json");
            File.WriteAllText(path, json);
        }
        
        /// <summary>
        /// 保存整个网格里的地图
        /// </summary>
        /// <param name="grid"></param>
        public void SaveWholeGrid()
        {
            SaveMapInfo();
            _tilemapManager.GetAllTilemaps().ForEach(map => SaveTileMap(map));
            SaveEntityData();
            _mapContext.Dirty = false;
        }

        public void LoadWholeGrid()
        {
            _tilemapManager.GetAllTilemaps().ForEach(map => LoadTileMap(map));
            LoadEntityData();
        }

        /// <summary>
        /// 保存整个地图
        /// </summary>
        /// <param name="map"></param>
        /// <param name="layerName"></param>
        public void SaveTileMap(Tilemap map, string layerName = null)
        {
            if (!map)
            {
                Debug.LogError("To save map is null."); return;
            }
            if (string.IsNullOrEmpty(layerName)) layerName = map.name;
            map.CompressBounds();
            var data = new TileDataArrayDict();
            data.ImportFromMap(map, map.cellBounds);
            string jsonText = JsonConvert.SerializeObject(data, jsonFormat);
            var directory = _mapContext.TileDataDirectory;
            Directory.CreateDirectory(directory);
            var fullPath = Path.Combine(directory, $"{layerName}.json");
            File.WriteAllText(fullPath, jsonText);
        }

        /// <summary>
        /// 载入整个地图
        /// </summary>
        /// <param name="map"></param>
        /// <param name="layerName"></param>
        public void LoadTileMap(Tilemap map, string layerName = null)
        {
            if (!map)
            {
                Debug.LogError("To save map is null."); return;
            }
            if (string.IsNullOrEmpty(layerName)) layerName = map.name;
            var fullPath = Path.Combine(_mapContext.TileDataDirectory, $"{layerName}.json");
            string jsonText;
            try
            {
                jsonText = File.ReadAllText(fullPath);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("Save not found.");
                return;
            }

            var data = JsonConvert.DeserializeObject<TileDataArrayDict>(jsonText);
            map.ClearAllTiles();
            data.ExportToMap(map, data.originPos, tileTable);
            map.CompressBounds();
        }

        /// <summary>
        /// 保存实体数据
        /// </summary>
        public void SaveEntityData()
        {
            // 取表
            var entities = _tilemapManager.GetAllGameObjects();
            var path = _mapContext.EntityDataPath;
            // 存文件
            File.WriteAllText(path, JsonConvert.SerializeObject(entities, Formatting.Indented));
        }

        /// <summary>
        /// 加载实体数据
        /// </summary>
        public void LoadEntityData()
        {
            var path = _mapContext.EntityDataPath;
            List<EntityData> entities;
            try
            {
                entities = JsonConvert.DeserializeObject<List<EntityData>>(File.ReadAllText(path));
            }
            catch (FileNotFoundException)
            {
                Debug.LogError($"Entity data not found! check if the path is valid: {path}");
                return;
            }
            if (entities != null)
            {
                _tilemapManager.RemoveAllGameObject();
                var instanceArray = new List<GameObject>(entities.Count);
                foreach (var entity in entities)
                {
                    // 添加记录的物体, 并记录实例化后的 GO
                    instanceArray.Add(_tilemapManager.AddGameObject(entity.position, (GameObject)entityTable[entity.prefabName]));
                }
                // 应用额外的数据
                for (var i = 0; i < entities.Count; i++)
                {
                    entities[i].ApplyExtraData(instanceArray[i]);
                }
            }
            else
            {
                Debug.Log("Entity data is empty!");
            }
        }
    }

}
