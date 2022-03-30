using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Prototype
{
    /// <summary>
    /// 用来输出 Debug 信息的元素
    /// </summary>
    public class DebugElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public bool logOnClick;
        public bool logOnEnter;
        public bool logOnExit;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(logOnClick) Debug.Log($"<b>{name}</b> is clicked.", gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(logOnEnter) Debug.Log($"Pointer entered <b>{name}</b>", gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(logOnExit) Debug.Log($"Pointer exited <b>{name}</b>", gameObject);
        }
    }

}
