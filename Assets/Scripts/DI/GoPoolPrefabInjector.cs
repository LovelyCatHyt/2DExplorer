using System.Collections;
using System.Collections.Generic;
using Unitilities;
using UnityEngine;
using Zenject;

namespace DI
{
    /// <summary>
    /// Go Pool 预制体自动注入器 不是为对象池注入, 而是为对象池中实例化的对象注入
    /// </summary>
    public class GoPoolPrefabInjector : MonoBehaviour
    {
        [Inject]
        private void PoolInit(DiContainer container, GameObjectPool pool)
        {
            pool.onPrefabInstantiate.AddListener(container.InjectGameObject);
        }
    }

}
