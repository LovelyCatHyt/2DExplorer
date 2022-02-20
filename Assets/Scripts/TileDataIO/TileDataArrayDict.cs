using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileDataIO
{
    public class TileDataArrayDict : AbstractTileDataFormat
    {
        /// <summary>
        /// 含义等同于 BoundsInt 的 size
        /// </summary>
        public Vector3Int size;
        /// <summary>
        /// 存储 Tile 的 ID
        /// </summary>
        public int[,] idArray;
        // public Dictionary<string, int[,]> extraData;

        public override void ImportFromMap(Tilemap map, BoundsInt region)
        {
            Init_Internal();
            originPos = region.position;
            size = region.size;
            idArray = new int[size.y + 1, size.x + 1];
            foreach (var pos in region.allPositionsWithin)
            {
                idArray[pos.y - region.y, pos.x - region.x] = GetIDOfTile(map.GetTile(pos));
            }
            Debug.Log($"Data extracted from {map.gameObject.name} at region {region}");
        }

        public override void ExportToMap(Tilemap map, Vector3Int origin, Dictionary<string, TileBase> tileLUT)
        {
            BuildTileList(tileLUT);
            var bounds = new BoundsInt(origin, size);
            foreach (var pos in bounds.allPositionsWithin)
            {
                map.SetTile(pos, _tileList[idArray[pos.y - origin.y, pos.x - origin.x]]);
            }
            Debug.Log($"Data loaded to {map.gameObject.name} at region {new BoundsInt(origin, size)}");
        }
    }

}