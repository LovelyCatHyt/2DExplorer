using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TileDataIO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Tiles
{
    /// <summary>
    /// 瓦片地图管理器, 管理所有层的瓦片, 同时提供一些对所有层生效的接口
    /// </summary>
    public class TilemapManager : MonoBehaviour
    {

        /// <summary>
        /// 地图数据管理器
        /// </summary>
        [Inject] private TileDataMgr _dataMgr;
        [Inject] private DiContainer _container;
        /// <summary>
        /// 瓦片地图列表. 序列化是为了保证序号的稳定性
        /// </summary>
        [SerializeField] private Tilemap[] _maps;
        private IGridMap<List<GameObject>> _goMap = new SparseGridMap<List<GameObject>>();

        [ContextMenu("Auto set maps")]
        public void AutoSetMaps()
        {
            _maps = GetComponentsInChildren<Tilemap>();
        }

        /// <summary>
        /// 获取指定坐标的单元格
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GridCell GetCell(Vector3Int position)
        {
            var cell = new GridCell();
            cell.tiles = _maps.Select(map => map.GetTile(position)).ToArray();
            cell.gameObjects = _goMap[position]?.ToArray() ?? new GameObject[0];
            return cell;
        }

        /// <summary>
        /// 设置指定坐标的单元格
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cell"></param>
        private void SetCellInternal(Vector3Int position, GridCell cell)
        {
            TileBase[] toSetTiles = new TileBase[_maps.Length];
            if (cell.tiles != null && cell.tiles.Any())
            {
                for (var i = 0; i < toSetTiles.Length && i < cell.tiles.Length; i++)
                {
                    toSetTiles[i] = cell.tiles[i];
                }
            }

            for (var i = 0; i < _maps.Length; i++)
            {
                _maps[i].SetTile(position, toSetTiles[i]);
            }
            
            _goMap[position] = cell.gameObjects?.ToList()?? new List<GameObject>();
        }

        /// <summary>
        /// 在指定坐标设置单元格. 从 <see cref="cell"/> 中获取的 GameObject 实例化到对应的位置
        /// <para>如果已经存在关联 GO, 则销毁关联的 GO</para>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cell"></param>
        public void SetCell(Vector3Int position, GridCell cell)
        {
            var originCell = GetCell(position);
            var worldPosition = _maps[0].GetCellCenterWorld(position);

            // 先实例化 再销毁
            // 由于数组是引用类型, 需要复制到一个新的列表中
            var goList = new GameObject[cell.gameObjects.Length];
            for (var i = 0; i < cell.gameObjects.Length; i++)
            {
                var prefab = cell.gameObjects[i];
                var go = Instantiate(prefab, worldPosition, Quaternion.identity, transform);
                _container.InjectGameObject(go);
                goList[i] = go;
            }

            cell.gameObjects = goList;

            foreach (var go in originCell.gameObjects)
            {
                if(go) Destroy(go);
            }

            SetCellInternal(position, cell);
        }

        /// <summary>
        /// 在指定坐标添加一个物体的关联
        /// </summary>
        /// <param name="position"></param>
        /// <param name="go"></param>
        public void AddGameObject(Vector3Int position, GameObject go)
        {
            // 没必要添加一个 null 的 GO
            if (!go) return;
            var list = _goMap[position] ??= new List<GameObject>();
            if (!list.Contains(go)) list.Add(go);
        }

        /// <summary>
        /// 关联一个物体到它所在的坐标上
        /// </summary>
        /// <param name="go"></param>
        public void AddGameObject(GameObject go)
        {
            AddGameObject(WorldToCell(go.transform.position), go);
        }

        /// <summary>
        /// 在指定坐标移除一个物品的关联
        /// </summary>
        /// <param name="position"></param>
        /// <param name="go"></param>
        public void RemoveGameObject(Vector3Int position, GameObject go) => _goMap[position].Remove(go);

        /// <summary>
        /// 获取单元坐标
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return !_maps.Any() ? new Vector3Int() : _maps[0].WorldToCell(worldPosition);
        }
    }

}
