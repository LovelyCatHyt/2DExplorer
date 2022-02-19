using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileTool
{
    /// <summary>
    /// ��Ƭ��ͼ������
    /// </summary>
    public interface ITilemapWalker
    {
        /// <summary>
        /// �Ӹ��������꿪ʼ����һ������ͬ tile ������.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="startPosition"></param>
        /// <param name="tile"></param>
        /// <param name="onTile"></param>
        public void Walk(Tilemap tilemap, Vector3Int startPosition, TileBase tile, Action<Tilemap, Vector3Int> onTile);
    }

    /// <summary>
    /// �׳��쳣��ֹͣ��Ƭ��ͼ����
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