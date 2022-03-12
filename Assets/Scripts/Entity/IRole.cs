using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    /// <summary>
    /// 角色接口
    /// </summary>
    public interface IRole
    {
        /// <summary>
        /// 所处的坐标
        /// </summary>
        public Transform RoleTransform { get; }
        /// <summary>
        /// 受一定伤害数值
        /// </summary>
        /// <param name="value"></param>
        public void Damaged(double value);
        /// <summary>
        /// 治疗一定的数值
        /// </summary>
        /// <param name="value"></param>
        public void Heal(double value);
        /// <summary>
        /// 按比例治疗
        /// </summary>
        /// <param name="value"></param>
        public void HealByRatio(double value);
    }

}
