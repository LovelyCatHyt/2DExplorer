using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// 地图数据文件夹相对路径
        /// </summary>
        public const string TileDataDirectoryRelative
#if UNITY_EDITOR
        = "../TileData/";
#else
        = "TileData/";
#endif
        public const string EntityDataPathRelative
#if UNITY_EDITOR
        = "../TileData/Entities.json";
#else
        = "TileData/Entities.json";
#endif

        [Inject] private TilemapManager _tilemapManager;

        /// <summary>
        /// 保存整个网格里的地图
        /// </summary>
        /// <param name="grid"></param>
        public void SaveWholeGrid()
        {
            _tilemapManager.GetAllTilemaps().ForEach(map => SaveTileMap(map));
            SaveEntityData();
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
            var directory = Path.Combine(Application.dataPath, TileDataDirectoryRelative);
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
            var fullPath = Path.Combine(Application.dataPath, TileDataDirectoryRelative, $"{layerName}.json");
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
            var path = Path.Combine(Application.dataPath, EntityDataPathRelative);
            // 存文件
            File.WriteAllText(path, JsonConvert.SerializeObject(entities, Formatting.Indented));
        }

        /// <summary>
        /// 加载实体数据
        /// </summary>
        public void LoadEntityData()
        {
            var path = Path.Combine(Application.dataPath, EntityDataPathRelative);
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

                foreach (var entity in entities)
                {
                    // 添加记录的物体, 并应用相关数据
                    _tilemapManager.AddGameObject(entity.position, (GameObject)entityTable[entity.prefabName], o => entity.ApplyExtraData(o));
                }
            }
            else
            {
                Debug.Log("Entity data is empty!");
            }
        }
    }

}
