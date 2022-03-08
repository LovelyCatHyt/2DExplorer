# 2D 探索游戏
## 游戏规则
操纵主角在地图上避开机关的攻击, 找到出口.

## 项目结构

### 文件目录

仅描述 Assets 文件夹.

- Audios: 音频文件
- GameFramework: 导入的 **GameFramework** 包
- Materials: 大部分材质球
- Plugins: 插件
- Prefabs: 预制体
- Resources: 仅 **DOTween** 使用
- Scenes: 场景
- Scripts: 脚本
- Settings: 记录游戏设置的 ScriptableObject
- Shaders: 用 ShaderGraph 制作的 shader
- Sprites: 2D Sprite
- Tiles: 瓦片模板和画板(编辑器用)

### 代码结构

该游戏核心代码程序集定义在[Scripts的根目录](Assets\Scripts)下, Editor 里是另一个程序集. 通过定义程序集的方式将项目中的代码切割为不同的编译对象, 减少代码变更时的编译代价, 同时也能强化各个模块的独立性, 明确依赖关系.

- CharCtrl: 角色控制. 操控刚体和按键映射
- DI: 依赖注入相关. 定义场景中的类的绑定关系
- Editor: 编辑器相关, 为某些类添加辅助功能
- EditorTool: 简化编辑器代码的一些工具
- Effect: 特效实现
- Entity: 实体. 如主角, 炮塔, 子弹
- Game: 游戏过程总的管理中心, 包括游戏状态的管理
- TileDataIO: 瓦片数据输入输出, 管理瓦片数据的序列化, 也包括瓦片所对应的外部 GO.
- Tiles: 瓦片类
- TileTool: 瓦片地图相关的一些工具类
- Unitilities: **Unitilities** 所在目录

### 引入的非官方包和插件

- ZenJect [GitHub链接](https://github.com/modesttree/Zenject) [本地插件路径](Assets\Plugins\Zenject): 围绕 Unity 的控制反转依赖注入模块
  - 摆脱简单单例模式带来的依赖顺序问题
  - 提供一个比 `Awake()` 更早的依赖注入, 对场景静态物体而言相当于增加了一个比 `Awake()`更早的事件
  - 面向接口的绑定关系
- DOTween [官网链接](http://dotween.demigiant.com/) [本地插件路径](Assets\Plugins\Demigiant\DOTween): 一个灵活而方便的动画过渡过程控制插件
  - 将空间变换, **Material** 属性变化, **Rigidbody** 运动过程等常用的动画过渡过程(下称 **Tween** )实现为扩展方法, 可以直接用一个方法触发
  - **Tween** 提供丰富控制参数, 如过渡方式
  - **Tween** 提供完善的控制方法和回调函数, 包括 `Kill()`, `OnComplete(callback)` 等
  - 易于实现连续动画过程, 或称序列 **Sequence**

- UnityGameFramework [GitHub链接](https://github.com/EllanJiang/UnityGameFramework) [本地插件路径](Assets\GameFramework): 一个包含常见业务逻辑和设计模式的框架
  - 有限状态机`IFsm<T>`实现了由状态机实例管理被管理的目标对象, 状态类仅实现逻辑不记录额外信息的状态机模式.
  - 事件系统实现了一个线程安全的事件订阅-发布机制, 使用整型 `id` 进行事件绑定
- Unitilities [GitHub链接](https://github.com/LovelyCatHyt/Unitilities) [本地路径](Assets\Scripts\Unitilities): 由 @LovelyCatHyt 制作的 Unity 的一些工具类
  - 很多零碎的小东西, 但也有比较独立的模块
  - 使用 git submodule 实现源码级依赖
  - 随本项目的开发而不定期更新