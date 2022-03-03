using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tiles
{
    /// <summary>
    /// 为了实现动态添加坐标, 甚至支持负数坐标, 记录坐标与点的序列的实现是最简单的. 该稀疏网格地图就是记录坐标和对象的序列.
    /// <para>不进行任何优化. 查询O(n), 写入O(n)</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SparseGridMap<T> : IGridMap<T> where T : class
    {
        private List<(Vector3Int, T)> _objList = new List<(Vector3Int, T)>();
        public T GetObject(Vector3Int position)
        {
            return _objList.Where(tuple => tuple.Item1 == position).Select(tuple => tuple.Item2).FirstOrDefault();
        }

        public void SetObject(Vector3Int position, T obj)
        {
            var i = _objList.FindIndex(t => t.Item1 == position);
            if (i >= 0)
            {
                _objList[i] = (position, obj);
            }
            else
            {
                _objList.Add((position, obj));
            }
        }

        public T this[Vector3Int pos]
        {
            get => GetObject(pos);
            set => SetObject(pos, value);
        }

        public IEnumerable<(Vector3Int, T)> GetEnumerable() => _objList.ToList();
    }
}
