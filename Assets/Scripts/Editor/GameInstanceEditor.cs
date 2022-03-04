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

            var game = (GameInstance)target;

            if (EditorApplication.isPlaying)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Save"))
                    {
                        game.SaveGame();
                    }

                    if (GUILayout.Button("Load"))
                    {
                        game.LoadGame();
                    }
                }


                var showPauseOrResume = game.State == GameState.Paused || game.State == GameState.InGame;
                if (showPauseOrResume && GUILayout.Button(game.State == GameState.Paused ? "Resume" : "Paused"))
                {
                    if (game.State == GameState.Paused)
                    {
                        game.Resume();
                    }
                    else
                    {
                        game.Pause();
                    }
                }

            }
        }
    }
}
