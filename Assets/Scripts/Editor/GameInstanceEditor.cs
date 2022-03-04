using Game;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameInstance))]
    public class GameInstanceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var game = (GameInstance) target;

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Load"))
                {
                    game.LoadGame();
                }
            }
        }
    }
}
