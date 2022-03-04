using System.Collections;
using System.Collections.Generic;
using Audio;
using Entity;
using Game;
using TileDataIO;
using Tiles;
using Unitilities;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;
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
        public AudioManagerComponent audioManager;
        public TilemapManager tilemapManager;
        public TileDataMgr tileDataMgr;

        public override void InstallBindings()
        {
            Container.Bind<GameObjectPool>().WithId("Bullet Pool").FromInstance(bulletPool);
            Container.Bind<GameObjectPool>().WithId("Explosion Pool").FromInstance(explosionPool);
            Container.Bind<MainCharacter>().FromInstance(FindObjectOfType<MainCharacter>());
            Container.Bind<AudioManager>().FromInstance(audioManager.audioManager);
            Container.Bind<TilemapManager>().FromInstance(tilemapManager);
            Container.Bind<TileDataMgr>().FromInstance(tileDataMgr);
            // UnityGameFramework 系列
            Container.Bind<FsmComponent>().FromComponentInChildren();
            Container.Bind<BaseComponent>().FromComponentInChildren();
            Container.Bind<GameInstance>().FromComponentOnRoot();


            // 处理 GameObjectPool 的实例化操作, 使其实现自动注入
            bulletPool.onPrefabInstantiate.AddListener(Inject);
            explosionPool.onPrefabInstantiate.AddListener(Inject);
        }

        private void Inject(GameObject go) => Container.InjectGameObject(go);
    }

}
