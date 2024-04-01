using System;
using System.Collections.Generic;
using System.Reflection;
using Sapo.DI.Runtime.Behaviours;
using Sapo.DI.Runtime.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sapo.DI.Editor.Common
{
    public class InjectorRuntimeInfo
    {
        private readonly Dictionary<Type, object> _instances;
        private readonly Dictionary<Type, object> _parentInstances;

        public InjectorRuntimeInfo(SInjector injector)
        {
            var t = injector.GetType();
            
            var instancesField = t.GetField("_instances", BindingFlags.NonPublic | BindingFlags.Instance);

            _instances = (Dictionary<Type, object>)instancesField!.GetValue(injector);

            if (t.GetField("_parent", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(injector) is not
                SInjector parent) return;

            _parentInstances = (Dictionary<Type, object>)instancesField.GetValue(parent);
        }
        
        public void OnGUI()
        {
            EditorGUI.indentLevel++;

            var count = DrawInstances(_parentInstances, new Color(1, 0.5f, 0f));
            count += DrawInstances(_instances, new Color(0.5f, 1, 0f));

            if (count == 0)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField("No instances registered.", EditorStyles.miniBoldLabel);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUI.indentLevel--;
        }

        private int DrawInstances(Dictionary<Type, object> instances, Color color)
        {
            if (instances == null) return 0;
            
            var count = 0;
            foreach (var (type, instance) in instances)
            {
                var isNullOrDestroyedUnityObject = instance == null || instance is Object o && !o;
                if (isNullOrDestroyedUnityObject) continue;

                var c = GUI.color;
                GUI.color = color;
                GUI.Box(EditorGUI.IndentedRect(EditorGUILayout.BeginVertical()), "", EditorStyles.helpBox);
                GUI.color = c;
                GUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(4);
                
                EditorGUILayout.LabelField(type.Name, EditorStyles.boldLabel);

                EditorGUI.BeginDisabledGroup(true);
                if (instance is Object uo) EditorGUILayout.ObjectField(uo, uo.GetType(), true);
                else EditorGUILayout.LabelField(instance.ToString());
                EditorGUI.EndDisabledGroup();
                
                GUILayout.Space(2);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);
                EditorGUILayout.EndVertical();
                count++;
            }

            return count;
        }
    }
}