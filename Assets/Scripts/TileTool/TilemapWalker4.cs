using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileTool
{
    /// <summary>
    /// 4连通区域遍历器
    /// </summary>
    public class TilemapWalker4 : ITilemapWalker
    {
        private Tilemap _map;
        private Queue<Vector3Int> _toSearchQueue;
        /// <summary>
        /// 已探索的集合
        /// </summary>
        private HashSet<Vector3Int> _closedSet;
        private Action<Tilemap, Vector3Int> _onTile;
        private TileBase _tile;

        protected virtual Vector3Int[] Directions =>
        new Vector3Int[]{
            Vector3Int.right,
            Vector3Int.up,
            Vector3Int.left,
            Vector3Int.down
        };

        private void StartWalk()
        {
            // 可探索的点
            bool CanOpen(Vector3Int curr)
            {
                return _map.GetTile(curr) == _tile && !_closedSet.Contains(curr) && !_toSearchQueue.Contains(curr);
            }

            // 从一个虚函数中获取方向, 这样8连通只需要继承该类并重载 Directions 函数即可
            var directions = Directions;
            // BFS
            while (_toSearchQueue.Any())
            {
                // 从队列中提取一个点访问并记录
                var curr = _toSearchQueue.Dequeue();
                _closedSet.Add(curr);
                try
                {
                    _onTile?.Invoke(_map, curr);
                }
                catch (StopTilemapWalkException)
                {
                    return;
                }
                // 将四个方向上的点加入到列表
                foreach (var dir in directions)
                {
                    var neighbor = curr + dir;
                    if (CanOpen(neighbor))
                    {
                        _toSearchQueue.Enqueue(neighbor);
                    }
                }
            }
        }

        /// <summary>
        /// 清除所有搜索状态
        /// </summary>
        public void ClearStates()
        {
            _map = null;
            _toSearchQueue = null;
            _tile = null;
            _closedSet = null;
            _onTile = null;
        }

        /// <summary>
        /// 从给定的坐标开始遍历一个有相同 tile 的4连通区域.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="startPosition"></param>
        /// <param name="tile"></param>
        /// <param name="onTile"></param>
        public void Walk(Tilemap tilemap, Vector3Int startPosition, TileBase tile, Action<Tilemap, Vector3Int> onTile)
        {
            _map = tilemap;
            _toSearchQueue = new Queue<Vector3Int>();
            _toSearchQueue.Enqueue(startPosition);
            _closedSet = new HashSet<Vector3Int>();
            _tile = tile;
            _onTile = onTile;
            StartWalk();
        }

        /// <summary>
        /// 立刻遍历, 省略了手动实例化对象的过程
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="startPosition"></param>
        /// <param name="tile"></param>
        /// <param name="onTile"></param>
        public static void WalkNow(Tilemap tilemap, Vector3Int startPosition, TileBase tile,
            Action<Tilemap, Vector3Int> onTile)
        {
            var walker = new TilemapWalker4();
            walker.Walk(tilemap, startPosition, tile, onTile);
        }
    }

}