using System.Collections;
using System.Collections.Generic;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileDataIO
{
    /// <summary>
    /// 地图数据接口
    /// </summary>
    public interface ITileDataFormat
    {
        /// <summary>
        /// 导出数据到地图的指定点
        /// </summary>
        /// <param name="map">要导出的地图目标</param>
        /// <param name="origin">导出的原点</param>
        /// <param name="tileLut">瓦片表, 用它来创建瓦片实例</param>
        public void ExportToMap(Tilemap map, Vector3Int origin, ObjectRefTable tileLut);

        /// <summary>
        /// 从地图的指定位置导入
        /// </summary>
        /// <param name="map"></param>
        /// <param name="region"></param>
        public void ImportFromMap(Tilemap map, BoundsInt region);
        
    }

}
