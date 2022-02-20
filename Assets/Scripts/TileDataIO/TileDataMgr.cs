using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        public TileBase[] tileAssets; 
        public Formatting jsonFormat;
        /// <summary>
        /// 地图数据文件夹相对路径
        /// </summary>
        public string tileDataDirectoryRelative
#if UNITY_EDITOR
        = "../TileData/";
#else
        = "TileData/";
#endif

        /// <summary>
        /// 保存整个网格里的地图
        /// </summary>
        /// <param name="grid"></param>
        public void SaveWholeGrid(GameObject grid)
        {
            // 遍历每个子 Transform
            foreach (var tilemapTran in grid.transform)
            {
                var tilemap = ((Transform) tilemapTran).GetComponent<Tilemap>();
                SaveTileMap(tilemap);
            }
        }

        public void LoadWholeGrid(GameObject grid)
        {
            // 遍历每个子 Transform
            foreach (var tilemapTran in grid.transform)
            {
                var tilemap = ((Transform)tilemapTran).GetComponent<Tilemap>();
                LoadTileMap(tilemap);
            }
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
            var directory = Path.Combine(Application.dataPath, tileDataDirectoryRelative);
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
            var fullPath = Path.Combine(Application.dataPath, tileDataDirectoryRelative, $"{layerName}.json");
            var jsonText = "";
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
            var tileDict = tileAssets.ToDictionary(x => x.name, x => x);
            tileDict["null"] = null;
            map.ClearAllTiles();
            data.ExportToMap(map, data.originPos, tileDict);
            map.CompressBounds();
        }

    }

}