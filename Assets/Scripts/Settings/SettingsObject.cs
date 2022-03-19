using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Settings
{
    /// <summary>
    /// 设置集合的基类, 属性变更时调用相应的事件
    /// </summary>
    public class SettingsObject : ScriptableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

}
