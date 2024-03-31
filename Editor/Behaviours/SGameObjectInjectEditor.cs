using Sapo.SInject.Runtime.Behaviours;
using UnityEditor;

namespace Sapo.SInject.Editor.Behaviours
{
    [CustomEditor(typeof(SGameObjectInject))]
    internal class SGameObjectInjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component is used to automatically inject this gameObject.", MessageType.Info,
                true);
        }
    }
}