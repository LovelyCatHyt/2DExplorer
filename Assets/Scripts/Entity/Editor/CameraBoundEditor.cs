using UnityEditor;

namespace Entity.Editor
{
    [CustomEditor(typeof(CameraBoundSetter))]
    public class CameraBoundEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            CameraBoundSetter setter = (CameraBoundSetter)target;

        }
    }

}
