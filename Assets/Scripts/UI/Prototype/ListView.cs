using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Ui.Prototype
{
    /// <summary>
    /// 列表视图, 最简单的那种
    /// </summary>
    public class ListView : MonoBehaviour
    {
        public IList<ListElement> List => _list.ToArray();

        public int SelectedId
        {
            get => _selectedId;
            set
            {
                // 裁剪
                var tempId = Mathf.Clamp(value, 0, _list.Count - 1);
                // 非空才能执行后续的操作
                if (_list.Any())
                {
                    if (tempId != _selectedId)
                    {
                        // 取消选中原来选中的目标
                        if (_selected) _selected.OnDeSelected();
                        _selected = _list[tempId];
                        // 重设选中的id
                        _selectedId = tempId;
                        _selected.OnSelected();
                        onSelectionChanged?.Invoke(_selected);

                    }
                }
            }
        }
        public ListElement Selected
        {
            get => _selected;
            set
            {
                if (value == _selected) return;
                var id = _list.IndexOf(value);
                if (id == -1)
                {
                    _selected.GetComponent<ListElement>().OnDeSelected();
                    _selected = null;
                }
                else
                {
                    _selectedId = id;
                    _selected = value;
                }
                onSelectionChanged?.Invoke(_selected);
            }
        }
        public UnityEvent<ListElement> onSelectionChanged;


        /// <summary>
        /// 布局组件
        /// <para>但是暂时不知道有什么用</para>
        /// </summary>
        [SerializeField] private LayoutGroup _layout;
        [Inject] private DiContainer _container;
        private readonly List<ListElement> _list = new List<ListElement>();
        private int _selectedId;
        public ListElement _selected;

        private void Awake()
        {
            if (!_layout) _layout = GetComponent<LayoutGroup>();
            // TODO: remove debug below
            onSelectionChanged.AddListener(e =>
            {
                Debug.Log($"Selection changed to [{SelectedId}]: {(e ? e.name : "null")}", e ? e.gameObject : null);
            });
            // Debug
        }

        /// <summary>
        /// 实例化一个 prefab, 然后添加到列表中
        /// </summary>
        /// <param name="prefab"></param>
        public void Add(GameObject prefab)
        {
            var go = _container.InstantiatePrefab(prefab, transform);
            var id = _list.Count;
            var element = go.GetComponent<ListElement>();
            if (!element) element = go.AddComponent<ListElement>();
            element.SetListView(this, id);
            _list.Add(element);
            if (_list.Count == 1)
            {
                // 从无到有的变化还是要注意一下
                SelectedId = 0;
                _selected = _list[0];
                onSelectionChanged?.Invoke(Selected);
            }
        }

        /// <summary>
        /// 移除一个 GO
        /// </summary>
        /// <param name="target"></param>
        public void Remove(GameObject target)
        {
            var element = target.GetComponent<ListElement>();
            if (!element) return;
            Remove(element);
        }

        /// <summary>
        /// 移除一个元素
        /// </summary>
        /// <param name="element"></param>
        public void Remove(ListElement element)
        {
            var id = _list.IndexOf(element);
            if (id == -1) return;
            
            _list.RemoveAt(id);
            // 重设一下后面的 id
            for (int i = id; i < _list.Count; i++)
            {
                _list[i].Id = id;
            }

            Destroy(element.gameObject);

            SelectedId = SelectedId;
            if (_list.Count == 0)
            {
                onSelectionChanged?.Invoke(null);
            }
        }

        public void RemoveAt(int i)
        {
            if (i < 0 || i >= _list.Count) return;
            Remove(_list[i]);
        }

        [ContextMenu("Remove last")]
        public void RemoveLast()
        {
            RemoveAt(_list.Count - 1);
        }
    }

}
