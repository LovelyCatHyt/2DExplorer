using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    /// <summary>
    /// 网格单元, 包含每一层的瓦片和对应的外部物体
    /// </summary>
    [Serializable]
    public struct GridCell
    {
        /// <summary>
        /// 每一层的瓦片
        /// </summary>
        public TileBase[] tiles;
        /// <summary>
        /// 所关联的 GO
        /// </summary>
        public GameObject[] gameObjects;
    }

}
