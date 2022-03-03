using System.Collections.Generic;

namespace Entity
{
    /// <summary>
    /// 含额外数据的 MonoBehaviour
    /// </summary>
    public interface IHasExtraData
    {
        /// <summary>
        /// 额外数据字典
        /// <para>同一个 GO 上的额外数据不能有重复的键, 存在重复键时的行为是未定义的</para>
        /// </summary>
        public Dictionary<string, object> ExtraData { get; set; }
    }
}
