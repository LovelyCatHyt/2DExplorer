using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameEditor
{
    /// <summary>
    /// 瓦片地图的菜单
    /// </summary>
    public static class TilemapMenu
    {
        [MenuItem("2DExplorer/Compress all tilemaps")]
        public static void CompressAllTilemaps()
        {
            foreach (var tilemap in Object.FindObjectsOfType<Tilemap>())
            {
                tilemap.CompressBounds();
                EditorUtility.SetDirty(tilemap);
            }
            EditorSceneManager.MarkAllScenesDirty();
        }
    }

}
