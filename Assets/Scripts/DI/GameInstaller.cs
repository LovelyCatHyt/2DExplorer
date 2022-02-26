using System.Collections;
using System.Collections.Generic;
using Entity;
using Unitilities;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace DI
{
    /// <summary>
    /// Zenject 的 Installer, 不是安装程序的那个 Installer
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        public GameObjectPool bulletPool;
        public GameObjectPool explosionPool;

        public override void InstallBindings()
        {
            Container.Bind<GameObjectPool>().WithId("Bullet Pool").FromInstance(bulletPool);
            Container.Bind<GameObjectPool>().WithId("Explosion Pool").FromInstance(explosionPool);
            Container.Bind<MainCharacter>().FromInstance(FindObjectOfType<MainCharacter>());
            // 处理 GameObjectPool 的实例化操作, 使其实现自动注入
            bulletPool.onPrefabInstantiate.AddListener(Inject);
            explosionPool.onPrefabInstantiate.AddListener(Inject);
        }

        private void Inject(GameObject go) => Container.InjectGameObject(go);
    }

}
