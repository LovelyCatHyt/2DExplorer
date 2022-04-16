using System.Collections;
using System.Collections.Generic;
using System.IO;
using TileDataIO;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    
    public class MapListElement : MonoBehaviour
    {
        public string MapDir
        {
            get => _mapDir;
            private set
            {
                _mapDir = Path.GetFullPath(value);
                var id = _mapDir.LastIndexOf(Path.DirectorySeparatorChar);
                _text.text = id != -1 ? _mapDir.Substring(id + 1) : "Invalid";
            }
        }

        public MapInfo Info { get; private set; }

        [SerializeField] private Text _text;
        private string _mapDir;

        private void Awake()
        {
            if(!_text) _text = GetComponent<Text>();
        }

        public void SetInfo(MapInfo info, string mapDir)
        {
            Info = info;
            MapDir = Path.GetFullPath(mapDir);
        }
    }

}
