using UnityEditor;
using UnityEngine;

namespace Sapo.DI.Editor.Attributes
{
    [CustomEditor(typeof(Component), true)]
    internal class SRegisterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SRegisterGUI.DrawInfo(target);
            DrawDefaultInspector();
        }
    }
}