using Sapo.DI.Runtime.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Sapo.DI.Editor.Behaviours
{
    [CustomEditor(typeof(SSceneInject))]
    internal class SSceneInjectEditor : UnityEditor.Editor
    {
        private readonly GUIContent _tempContent = new GUIContent();
        

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component is used to automatically inject this scene.", MessageType.Info,
                true);
            
            _tempContent.image ??= EditorGUIUtility.IconContent("console.warnicon.sml").image;
            _tempContent.text = "This gameObject will be destroyed after the injection process.";
            EditorGUILayout.LabelField(GUIContent.none, _tempContent, EditorStyles.helpBox);
        }
    }
}