using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Entity 的 Editor
    /// </summary>
    [CustomEditor(typeof(Entity.Entity))]
    public class EntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var entity = (Entity.Entity)target;
            if (!entity.gameObject.scene.IsValid())
            {
                // 仅在 prefab 中有意义
                var prefabName = serializedObject.FindProperty("_prefabName");
                if (string.IsNullOrEmpty(prefabName.stringValue)) prefabName.stringValue = entity.gameObject.name;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
