using System;
using System.Collections.Generic;
using System.Reflection;
using Sapo.SInject.Editor.Common;
using Sapo.SInject.Runtime.Attributes;
using Sapo.SInject.Runtime.Behaviours;
using Sapo.SInject.Runtime.Common;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sapo.SInject.Editor.Behaviours
{
    [CustomEditor(typeof(SRootInjector))]
    internal class SRootInjectorEditor : UnityEditor.Editor
    {
        private SRootInjector _injector;
        private SerializedProperty _makePersistent;
        private SerializedProperty _assetsToRegister;
        private ReorderableList _assetsToRegisterList;

        private Dictionary<Type, object> _injectorInstances;

        private PrefsBool _settingsFoldout = new("Sapo.DI.Editor.Behaviours.SRootInjectorEditor._settingsFoldout");
        private PrefsBool _runtimeInfoFoldout = new("Sapo.DI.Editor.Behaviours.SRootInjectorEditor._runtimeInfoFoldout");


        private void OnEnable()
        {
            _injector = (SRootInjector)target;

            _makePersistent = serializedObject.FindProperty("makePersistent");
            _assetsToRegister = serializedObject.FindProperty("assetsToRegister");
            
            _settingsFoldout.Load();
            _runtimeInfoFoldout.Load();

            _assetsToRegisterList = new ReorderableList(serializedObject, _assetsToRegister, true, true, true, true);
            _assetsToRegisterList.drawHeaderCallback = rect => GUI.Label(rect, "Assets to register");
            _assetsToRegisterList.drawElementCallback = (rect, index, active, focused) =>
            {
                rect.y += 2;
                rect.height -= 4;
                var element = _assetsToRegister.GetArrayElementAtIndex(index);
                
                EditorGUI.BeginChangeCheck();
                EditorGUI.ObjectField(rect, element, GUIContent.none);
                if (!EditorGUI.EndChangeCheck()) return;
                if (element.objectReferenceValue == null) return;
                if (element.objectReferenceValue.GetType().IsDefinedWithAttribute<SRegister>()) return;

                element.objectReferenceValue = null;
            };
            _assetsToRegisterList.elementHeight = 20;
        }

        public override bool RequiresConstantRepaint() => true;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                "This component is a root injector that will be used to register and inject dependencies.",
                MessageType.Info, true);
            
            serializedObject.UpdateIfRequiredOrScript();

            DrawSettingsGUI();
            DrawRuntimeGUI();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSettingsGUI()
        {
            _settingsFoldout.Value = EditorGUILayout.Foldout(_settingsFoldout.Value, "Settings", true);
            if (!_settingsFoldout.Value) return;

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_makePersistent);

            var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, _assetsToRegisterList.GetHeight()));
            _assetsToRegisterList.DoList(rect);
            
            EditorGUI.indentLevel--;

        }

        private void DrawRuntimeGUI()
        {
            var isPlaying = Application.IsPlaying(_injector);

            if (!isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Foldout(false, "Runtime info", true);
                EditorGUI.EndDisabledGroup();
                return;
            }
            
            
            _runtimeInfoFoldout.Value = EditorGUILayout.Foldout(_runtimeInfoFoldout.Value, "Runtime info", true);
            if (!_runtimeInfoFoldout.Value) return;
            
            

            _injectorInstances ??= (Dictionary<Type, object>)typeof(SInjector)
                .GetField("_instances", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(_injector);
            
            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);

            var count = 0;
            foreach (var (type, instance) in _injectorInstances)
            {
                var isNullOrDestroyedUnityObject = instance == null || instance is Object o && !o;
                if (isNullOrDestroyedUnityObject) continue;
                
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(type.Name, EditorStyles.boldLabel);

                EditorGUI.BeginDisabledGroup(true);
                if (instance is Object uo) EditorGUILayout.ObjectField(uo, uo.GetType(), true);
                else EditorGUILayout.LabelField(instance.ToString());
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();
                count++;
            }

            if (count == 0)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.LabelField("No instances registered.", EditorStyles.miniBoldLabel);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}