using System.IO;
using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui
{
    /// <summary>
    /// 地图名字的 UI
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class MapNameUi : MonoBehaviour
    {
        [Inject] private MapContext _mapContext;
        private Text _text;

        // Start is called before the first frame update
        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void Start()
        {
            _text.text = _mapContext.Info.displayName;
        }
    }


}
