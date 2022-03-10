using System;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using Zenject;

namespace Entity
{
    /// <summary>
    /// 外部 GO, 绑定到地图上的可保存物体
    /// </summary>
    public class ExternalObject : MonoBehaviour
    {
        [Inject]
        private void Init(TilemapManager manager)
        {
            manager.RecordGameObject(gameObject);
        }
    }

}
