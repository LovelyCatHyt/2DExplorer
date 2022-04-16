using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Prototype
{
    /// <summary>
    /// 列表(<see cref="ListView"/>)中的元素
    /// </summary>
    [DisallowMultipleComponent]
    public class ListElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Color normalColorA = new Color(.976470f, .976470f, .976470f);
        public Color normalColorB = new Color(1, 1, 1);
        public Color highLightColor = new Color(.898039f, .898039f, .898039f);
        public Color selectedColor = new Color(.945098f, .945098f, .945098f);
        public Image image;
        public UnityAction<GameObject> onSelected;
        public UnityAction<GameObject> onDeSelected;
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private ListView _listView;
        private int _id;
        private bool _selected = false;
        private bool _pointerStay = false;

        private void Update()
        {
            RefreshImage();
        }

        private void RefreshImage()
        {
            if (!image) return;
            if (_pointerStay)
            {
                image.color = highLightColor;
            }
            else
            {
                if (_selected)
                {
                    image.color = selectedColor;
                }
                else
                {
                    // 奇偶交替
                    image.color = (Id & 1) == 0 ? normalColorA : normalColorB;
                }
            }
        }

        public void SetListView(ListView listView, int id)
        {
            _listView = listView;
            _id = id;
        }

        public void OnDeSelected()
        {
            onDeSelected?.Invoke(gameObject);
            _selected = false;
        }

        public void OnSelected()
        {
            onSelected?.Invoke(gameObject);
            _selected = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _listView.SelectedId = Id;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerStay = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerStay = false;
        }
    }

}
