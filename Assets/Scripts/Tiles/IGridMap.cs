using System;
using UnityEngine;

namespace Tiles
{
    /// <summary>
    /// 保存 <see cref="T"/> 的网格地图接口. 由于矩阵网格的特殊性, 稠密矩阵和系数矩阵的实现可能截然不同. 但对外界而言可以提取出共同的接口.
    /// </summary>
    public interface IGridMap<T> where T: class
    {
        /// <summary>
        /// 获取给定坐标的对象
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public T GetObject(Vector3Int position);

        /// <summary>
        /// 设置给定坐标的对象
        /// </summary>
        /// <param name="position"></param>
        /// <param name="obj"></param>
        public void SetObject(Vector3Int position, T obj);
        
        public T this [Vector3Int pos] { get; set; }
    }
}
