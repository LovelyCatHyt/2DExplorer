using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileDataIO
{
    public abstract class AbstractTileDataFormat : ITileDataFormat
    {
        /// <summary>
        /// ԭ��
        /// </summary>
        public Vector3Int originPos;
        /// <summary>
        /// �� Tile �����ֲ������ݱ��ڵ������Ĳ��ұ�, ���ڴӵ�ͼ��������
        /// </summary>
        protected Dictionary<string, int> _tileName2ID_LUT = new Dictionary<string, int>();
        /// <summary>
        /// ������ֱ�ӻ�ȡ Tile ������, ����Ҫ���������
        /// </summary>
        [JsonProperty("tileNames")]
        protected List<string> _tileNameList = new List<string>();
        /// <summary>
        /// ������ֱ�ӻ�ȡ TileBase ��ʵ��, ���ڴ����ݻ�ԭ��ͼ
        /// </summary>
        protected List<TileBase> _tileList = new List<TileBase>();

        protected void Init_Internal()
        {
            _tileName2ID_LUT.Add("null", 0);
            _tileNameList.Add("null");
        }

        /// <summary>
        /// ���� TileList
        /// </summary>
        /// <param name="tileLUT"></param>
        protected void BuildTileList(Dictionary<string, TileBase> tileLUT)
        {
            // "null" �������Ƭ
            tileLUT["null"] = null;
            for (var i = 0; i < _tileNameList.Count; i++)
            {
                if(tileLUT.TryGetValue(_tileNameList[i], out var tile))
                {
                    _tileList.Add(tile);
                }
                else
                {
                    Debug.LogError($"{_tileNameList[i]} not found in tileLUT. please check the tileLUT or the tileNames in the data.");
                    _tileList.Add(null);
                }
            }
        }

        /// <summary>
        /// �� ID ��ȡһ�� Tile ������
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
        /// ��ȡ��Ƭ������ʵ���ж�Ӧ��ID
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        protected int GetIDOfTile(TileBase tile)
        {
            if (tile == null) return 0;
            // ����û����� Tile �ͼ�¼һ��
            if (!_tileName2ID_LUT.ContainsKey(tile.name))
            {
                _tileName2ID_LUT.Add(tile.name, _tileNameList.Count);
                _tileNameList.Add(tile.name);
            }
            return _tileName2ID_LUT[tile.name];
        }

        public abstract void ImportFromMap(Tilemap map, BoundsInt region);
        public abstract void ExportToMap(Tilemap map, Vector3Int origin, Dictionary<string, TileBase> tileLUT);
    }

}