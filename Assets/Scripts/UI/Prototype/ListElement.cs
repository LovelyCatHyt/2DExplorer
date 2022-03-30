using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Ui.Prototype
{
    /// <summary>
    /// 列表(<see cref="ListView"/>)中的元素
    /// </summary>
    [DisallowMultipleComponent]
    public class ListElement : MonoBehaviour, IPointerClickHandler
    {
        public UnityAction<GameObject> onSelected;
        public UnityAction<GameObject> onDeSelected;
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private ListView _listView;
        private int _id;

        public void SetListView(ListView listView, int id)
        {
            _listView = listView;
            _id = id;
        }

        public void OnDeSelected()
        {
            onDeSelected?.Invoke(gameObject);
        }

        public void OnSelected()
        {
            onSelected?.Invoke(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _listView.SelectedId = Id;
        }
    }

}
