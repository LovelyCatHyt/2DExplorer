using System;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 实体, 具备一些通用的行为和信息
    /// </summary>
    [AddComponentMenu("GameMain/Entity")]
    public class Entity : MonoBehaviour
    {

        public string PrefabName
        {
            get => _prefabName;
            set => _prefabName = value;
        }

        /// <summary>
        /// 引用本身的 gameObject, 用于复制, 地图加载等需要唯一实例化源的目的
        /// </summary>
        [SerializeField] private string _prefabName;
    }
}
