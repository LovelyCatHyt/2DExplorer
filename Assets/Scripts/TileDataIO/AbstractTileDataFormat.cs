using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileDataIO
{
    public abstract class AbstractTileDataFormat : ITileDataFormat
    {
        /// <summary>
        /// 原点
        /// </summary>
        public Vector3Int originPos;
        /// <summary>
        /// 从 Tile 的名字查找数据表内的索引的查找表, 用于从地图保存数据
        /// </summary>
        protected Dictionary<string, int> _tileName2ID_LUT = new Dictionary<string, int>();
        /// <summary>
        /// 从索引直接获取 Tile 的名字, 是需要保存的数据
        /// </summary>
        [JsonProperty("tileNames")]
        protected List<string> _tileNameList = new List<string>();
        /// <summary>
        /// 从索引直接获取 TileBase 的实例, 用于从数据还原地图
        /// </summary>
        protected List<TileBase> _tileList = new List<TileBase>();

        protected void Init_Internal()
        {
            _tileName2ID_LUT.Add("null", 0);
            _tileNameList.Add("null");
        }

        /// <summary>
        /// 构建 TileList
        /// </summary>
        /// <param name="tileLut"></param>
        protected void BuildTileList(ObjectRefTable tileLut)
        {
            foreach (var tileName in _tileNameList)
            {
                try
                {
                    var tile = tileLut[tileName] as TileBase;
                    _tileList.Add(tile);
                }
                catch (KeyNotFoundException)
                {
                    Debug.LogError($"{tileName} not found in tileLUT. please check the tileLUT or the tileNames in the data.");
                    _tileList.Add(null);
                }
            }
        }

        /// <summary>
        /// 从 ID 获取一个 Tile 的名字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string GetTileNameFromID(int id)
        {
            if (id < 0) return "null";
            if (id >= _tileNameList.Count)
            {
                Debug.LogWarning("Tile ID exceeded in this Tile dataset. fallback to the first available tile.");
                return _tileNameList[0];
            }

            return _tileNameList[id];
        }

        /// <summary>
        /// 获取瓦片在数据实例中对应的ID
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        protected int GetIDOfTile(TileBase tile)
        {
            if (tile == null) return 0;
            // 表里没有这个 Tile 就记录一下
            if (!_tileName2ID_LUT.ContainsKey(tile.name))
            {
                _tileName2ID_LUT.Add(tile.name, _tileNameList.Count);
                _tileNameList.Add(tile.name);
            }
            return _tileName2ID_LUT[tile.name];
        }

        public abstract void ImportFromMap(Tilemap map, BoundsInt region);
        public abstract void ExportToMap(Tilemap map, Vector3Int origin, ObjectRefTable tileLut);
    }

}
