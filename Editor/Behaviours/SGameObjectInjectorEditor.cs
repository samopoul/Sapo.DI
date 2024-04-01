using Sapo.DI.Editor.Common;
using Sapo.DI.Runtime.Behaviours;
using Sapo.DI.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace Sapo.DI.Editor.Behaviours
{
    [CustomEditor(typeof(SGameObjectInjector))]
    public class SGameObjectInjectorEditor : UnityEditor.Editor
    {
        private SGameObjectInjector _injector;
        private RegistrableAssetsList _assetsToRegister;
        
        private readonly PrefsBool _settingsFoldout = new("Sapo.DI.Editor.Behaviours.SGameObjectInjectorEditor._settingsFoldout");
        private readonly PrefsBool _runtimeInfoFoldout = new("Sapo.DI.Editor.Behaviours.SGameObjectInjectorEditor._runtimeInfoFoldout");
        private InjectorRuntimeInfo _runtimeInfo;

        private void OnEnable()
        {
            _injector = (SGameObjectInjector)target;
            
            _assetsToRegister = new RegistrableAssetsList(serializedObject.FindProperty("assetsToRegister"));
            
            _settingsFoldout.Load();
            _runtimeInfoFoldout.Load();
        }

        public override void OnInspectorGUI()
        {
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

            _assetsToRegister.OnGUI();
            
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

            _runtimeInfo ??= new InjectorRuntimeInfo((SInjector)_injector.Injector);
            
            _runtimeInfo.OnGUI();
        }

    }
}