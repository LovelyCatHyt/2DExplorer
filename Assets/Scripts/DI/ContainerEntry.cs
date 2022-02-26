using System;
using UnityEngine;
using Zenject;

namespace DI
{
    /// <summary>
    /// IoC 容器访问口, 用于在某些情况下无法在初始化时访问 Container
    /// </summary>
    public class ContainerEntry : MonoBehaviour
    {
        public DiContainer Container => _container;

        [Inject] private DiContainer _container;
    }

}
