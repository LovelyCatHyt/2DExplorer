using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unitilities;
using Unitilities.Camera;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameEditor
{
    /// <summary>
    /// 摄像机工具
    /// </summary>
    public static class CameraMenu
    {
        [MenuItem("2DExplorer/Set Camera Bound as tilemap")]
        public static void SetBoundsAsTilemap()
        {
            SimpleCam2D cam = Object.FindObjectOfType<SimpleCam2D>();
            if(!cam) return;
            var maps = Object.FindObjectsOfType<Tilemap>();
            if (!maps.Any()) return;
            var bounds2D = new Bounds2D(maps[0].cellBounds.center, (Vector3)maps[0].cellBounds.size);
            foreach (var map in maps)
            {
                bounds2D = bounds2D.Encapsulate((Vector3)map.cellBounds.max);
                bounds2D = bounds2D.Encapsulate((Vector3)map.cellBounds.min);
            }
            
            if (bounds2D != cam.cameraBounds)
            {
                cam.cameraBounds = bounds2D;
                EditorUtility.SetDirty(cam);
                EditorSceneManager.MarkSceneDirty(cam.gameObject.scene);
            }
        }

        //private static BoundsInt BoundsCombine(BoundsInt a, BoundsInt b)
        //{
        //    var min_x = Mathf.Min(a.xMin, b.xMin);
        //    var min_y = Mathf.Min(a.yMin, b.yMin);
        //    var sizeX = Mathf.Max(a.xMax, b.xMax) - min_x;
        //    var sizeY = Mathf.Max(a.yMax, b.yMax) - min_y;
            
        //    return new BoundsInt(min_x, min_y, 0, sizeX, sizeY, 0);
        //}
    }

}
