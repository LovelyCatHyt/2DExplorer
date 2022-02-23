using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    /// <summary>
    /// 有 GameObject 的 Tile
    /// </summary>
    [CreateAssetMenu(fileName = "New Tile with Object", menuName = "Tile/Object Tile")]
    public class ObjectTile : Tile
    {
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            var ret = base.StartUp(position, tilemap, go);
            var mapCom = tilemap.GetComponent<Tilemap>();
            if (!go) return ret;
            var com = go.GetComponents<ITileGOComponent>();
            foreach (var tileGOComponent in com)
            {
                tileGOComponent.StartUp(position, mapCom);
            }
            return ret;
        }
    }

    /// <summary>
    /// 继承这个接口的组件可以获取一些瓦片相关的事件
    /// </summary>
    public interface ITileGOComponent
    {
        /// <summary>
        /// Tile.StartUp() 中调用
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        public void StartUp(Vector3Int position, Tilemap tilemap);
    }
}
