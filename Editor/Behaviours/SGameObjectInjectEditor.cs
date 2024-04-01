using Sapo.DI.Runtime.Behaviours;
using UnityEditor;

namespace Sapo.DI.Editor.Behaviours
{
    [CustomEditor(typeof(SPrefabInject))]
    internal class SGameObjectInjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component is used to automatically inject this gameObject.", MessageType.Info,
                true);
        }
    }
}