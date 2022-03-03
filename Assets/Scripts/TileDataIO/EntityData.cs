using System.Collections.Generic;
using System.Linq;
using Entity;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Tiles;
using UnityEngine;

namespace TileDataIO
{
    /// <summary>
    /// 外部的 GO 的数据, 外部是相对于 Tilemap上能获取的 GO 而言的, 需要通过 <see cref="TilemapManager"/> 获取的 GO. 用于保存地图.
    /// </summary>
    public struct EntityData
    {
        public Vector3Int position;

        [JsonProperty("name")]
        public string prefabName;

        /// <summary>
        /// 额外的数据
        /// </summary>
        private Dictionary<string, object> _extraData;

        /// <summary>
        /// 额外的数据
        /// </summary>
        [JsonProperty("extraData")]
        public Dictionary<string, object> ExtraData
        {
            get => _extraData;
            set => _extraData = value;
        }

        /// <summary>
        /// 从 GO 的含 <see cref="IHasExtraData"/> 接口的组件上获取相应的数据
        /// </summary>
        /// <param name="go"></param>
        public void AddExtraData(GameObject go)
        {
            var dict = new Dictionary<string, object>();
            foreach (var target in go.GetComponents<IHasExtraData>())
            {
                foreach (var kv in target.ExtraData)
                {
                    dict[kv.Key] = kv.Value;
                }
            }
            ExtraData = dict;
        }

        /// <summary>
        /// 将数据应用到指定的 GO 的含 <see cref="IHasExtraData"/> 接口的组件上
        /// </summary>
        /// <param name="go"></param>
        public void ApplyExtraData(GameObject go)
        {
            var targets = go.GetComponents<IHasExtraData>();
            foreach (var target in targets)
            {
                target.ExtraData = ExtraData;
            }
        }

        [UsedImplicitly]
        public bool ShouldSerializeExtraData()
        {
            return _extraData != null && _extraData.Any();
        }
    }
}
