using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileTool
{
    public class TilemapWalker8 : TilemapWalker4
    {
        protected override Vector3Int[] Directions =>
            new Vector3Int[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(-1, -1, 0),
                new Vector3Int(0, -1, 0),
                new Vector3Int(1, -1, 0)
            };

        /// <summary>
        /// 立刻遍历, 省略了手动实例化对象的过程
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="startPosition"></param>
        /// <param name="tile"></param>
        /// <param name="onTile"></param>
        public new static void WalkNow(Tilemap tilemap, Vector3Int startPosition, TileBase tile,
            Action<Tilemap, Vector3Int> onTile)
        {
            var walker = new TilemapWalker8();
            walker.Walk(tilemap, startPosition, tile, onTile);
        }
    }

}