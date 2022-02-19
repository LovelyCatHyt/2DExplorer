using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileTool
{
    /// <summary>
    /// 瓦片地图遍历器
    /// </summary>
    public interface ITilemapWalker
    {
        /// <summary>
        /// 从给定的坐标开始遍历一个有相同 tile 的区域.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="startPosition"></param>
        /// <param name="tile"></param>
        /// <param name="onTile"></param>
        public void Walk(Tilemap tilemap, Vector3Int startPosition, TileBase tile, Action<Tilemap, Vector3Int> onTile);
    }

    /// <summary>
    /// 抛出异常来停止瓦片地图遍历
    /// </summary>
    public class StopTilemapWalkException : Exception
    {
        public Vector3Int position;

        public StopTilemapWalkException(Vector3Int position)
        {
            this.position = position;
        }
    }
}