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
        public AudioManagerComponent audioManager;
        public TilemapManager tilemapManager;
        public TileDataMgr tileDataMgr;

        public override void InstallBindings()
        {
            Container.Bind<MainCharacter>().FromInstance(FindObjectOfType<MainCharacter>());
            Container.Bind<AudioManager>().FromInstance(audioManager.audioManager);
            Container.Bind<TilemapManager>().FromInstance(tilemapManager);
            Container.Bind<TileDataMgr>().FromInstance(tileDataMgr);
            // UnityGameFramework 系列
            Container.Bind<FsmComponent>().FromComponentInChildren();
            Container.Bind<BaseComponent>().FromComponentInChildren();
            Container.Bind<GameInstance>().FromComponentOnRoot().AsSingle();
            
        }
    }

}
