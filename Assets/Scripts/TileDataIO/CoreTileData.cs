using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileDataIO
{
    // 只存储瓦片类型的话, 似乎不需要更多具体的数据了. 因为其他数据都可以在 Tile 的实例中获取
    ///// <summary>
    ///// 核心的瓦片数据
    ///// </summary>
    //[Serializable]
    //public struct CoreTileData
    //{
    //    /// <summary>
    //    /// 瓦片的id
    //    /// </summary>
    //    public int tileID;
        
    //    /// <summary>
    //    /// 瓦片的变换
    //    /// </summary>
    //    public Matrix4x4 transform;

    //    /// <summary>
    //    /// 瓦片的颜色
    //    /// </summary>
    //    public Color color;
    //}

}