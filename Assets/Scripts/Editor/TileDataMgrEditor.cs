using TileDataIO;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor
{
    [CustomEditor(typeof(TileDataMgr))]
    public class TileDataMgrEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var mgr = (TileDataMgr)target;
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorGUILayout.HelpBox("Can not use help button at Editor mode.", MessageType.Info);
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Save"))
                {
                    mgr.SaveWholeGrid();
                }

                if (GUILayout.Button("Load"))
                {
                    mgr.LoadWholeGrid();
                }
            }
        }
    }
}
