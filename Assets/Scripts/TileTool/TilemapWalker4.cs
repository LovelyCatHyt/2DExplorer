using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileTool
{
    /// <summary>
    /// 4��ͨ���������
    /// </summary>
    public class TilemapWalker4 : ITilemapWalker
    {
        private Tilemap _map;
        private Queue<Vector3Int> _toSearchQueue;
        /// <summary>
        /// ��̽���ļ���
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
            // ��̽���ĵ�
            bool CanOpen(Vector3Int curr)
            {
                return _map.GetTile(curr) == _tile && !_closedSet.Contains(curr) && !_toSearchQueue.Contains(curr);
            }

            // ��һ���麯���л�ȡ����, ����8��ֻͨ��Ҫ�̳и��ಢ���� Directions ��������
            var directions = Directions;
            // BFS
            while (_toSearchQueue.Any())
            {
                // �Ӷ�������ȡһ������ʲ���¼
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
                // ���ĸ������ϵĵ���뵽�б�
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
        /// �����������״̬
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
        /// �Ӹ��������꿪ʼ����һ������ͬ tile ��4��ͨ����.
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
        /// ���̱���, ʡ�����ֶ�ʵ��������Ĺ���
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