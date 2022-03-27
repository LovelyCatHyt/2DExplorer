using System.Collections;
using System.Collections.Generic;
using Map;
using Settings;
using UnityEngine;
using Zenject;

namespace DI
{
    /// <summary>
    /// 项目安装器
    /// </summary>
    public class ProjectInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<SettingsManager>().AsSingle();
            Container.Bind<MapContext>().AsSingle();
        }
    }

}
