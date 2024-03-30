using Sapo.DI.Runtime.Attributes;
using Sapo.DI.Runtime.Common;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sapo.DI.Editor.Attributes
{
    public class SRegisterGUI
    {
        private static GUIStyle _style;
        
        public static void DrawInfo(Object target)
        {
            if (!target.GetType().IsDefinedWithAttribute<SRegister>(out var attribute)) return;
            
            _style ??= new GUIStyle(EditorStyles.helpBox) { richText = true };
            EditorGUILayout.LabelField($"Component registered as <color=#FF8000>{attribute.Type.Name}</color>", _style);
        }
    }
}