using Sapo.DI.Runtime.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Sapo.DI.Editor.Behaviours
{
    internal class EditorMenuActions
    {
        [MenuItem("GameObject/DI/Root Injector", false, 1)]
        public static void CreateRootInjector()
        {
            var g = new GameObject("Root Injector", typeof(SRootInjector));
            g.transform.hideFlags = HideFlags.HideInInspector;
            
            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Create Root Injector");
        }

        [MenuItem("GameObject/DI/Root Injector", true)]
        public static bool ValidateCreateRootInjector() => Object.FindObjectOfType<SRootInjector>() == null;
        
        [MenuItem("GameObject/DI/Scene Inject", false, 2)]
        public static void CreateSceneInject()
        {
            var g = new GameObject("Scene Inject", typeof(SSceneInject));
            g.transform.hideFlags = HideFlags.HideInInspector;
            
            Selection.activeGameObject = g;
            Undo.RegisterCreatedObjectUndo(g, "Create Scene Inject");
        }
        
        [MenuItem("GameObject/DI/Scene Inject", true)]
        public static bool ValidateCreateSceneInject() => Object.FindObjectOfType<SSceneInject>() == null;
    }
}