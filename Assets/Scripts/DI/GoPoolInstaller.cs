using System.Collections;
using System.Collections.Generic;
using Game;
using Unitilities;
using Unitilities.DebugUtil;
using UnityEngine;
using Zenject;

namespace DI
{
    /// <summary>
    /// Go Pool 的 Installer
    /// </summary>
    [AddComponentMenu("GameMain/DI/GoPool Installer")]
    public class GoPoolInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log(Container.ParentContainers[0].HasBinding<GameInstance>());
            Container.Bind<GameObjectPool>().FromComponentOnRoot();
        }
    }

}
