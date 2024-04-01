using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Sapo.DI.Runtime.Behaviours;
using Sapo.DI.Tests.Runtime.TestData;
using UnityEngine;
using UnityEngine.TestTools;

namespace Sapo.DI.Tests.Runtime.Behaviours
{
    [TestFixture]
    public class SGameObjectInjectorTests
    {
        private readonly List<GameObject> _gameObjects = new();
        
        private void AddAssetToRegister(SGameObjectInjector injector, Object asset)
        {
            var field = injector.GetType().GetField("assetsToRegister", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) throw new AssertionException("Field not found");
            
            var list = (List<Object>) field.GetValue(injector);
            list.Add(asset);
        }
        
        [TearDown]
        public void TearDown()
        {
            foreach (var go in _gameObjects)
            {
                if (go == null) continue;

                Object.DestroyImmediate(go);
            }
            _gameObjects.Clear();
        }

        private GameObject NewG(bool active = true)
        {
            var g = new GameObject();
            g.SetActive(active);
            _gameObjects.Add(g);
            return g;
        }

        #region Awake

        
        [UnityTest]
        private IEnumerator Awake_ShouldNotThrow()
        {
            // Arrange
            var g = NewG();
            
            // Act
            void Act() => g.AddComponent<SGameObjectInjector>();
            yield return null;
            
            // Assert
            Assert.That(Act, Throws.Nothing);
        }
        
        [UnityTest]
        private IEnumerator Awake_WithAssetsToRegister_ShouldRegister()
        {
            // Arrange
            var injector = NewG(false).AddComponent<SGameObjectInjector>();
            var service = ScriptableObject.CreateInstance<ScriptableObject>();
            AddAssetToRegister(injector, service);
            
            // Act
            injector.gameObject.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(injector.Injector.IsRegistered<IServiceA>(), Is.True);
            Assert.That(injector.Injector.Resolve<IServiceA>(), Is.EqualTo(service));
        }
        
        [UnityTest]
        public IEnumerator Awake_OnServices_ShouldCallEvents_WithCorrectOrder()
        {
            // Arrange
            var injector = NewG(false).AddComponent<SGameObjectInjector>();
            var service = ScriptableObject.CreateInstance<ScriptableServiceAWithEvents>();
            AddAssetToRegister(injector, service);
            
            // Act
            injector.gameObject.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(service.Events, Is.EqualTo(new[] { "OnRegister", "OnInject" }));
        }
        
        [UnityTest]
        public IEnumerator Awake_WithNullAssetToRegister_ShouldPass()
        {
            // Arrange
            var injector = NewG(false).AddComponent<SGameObjectInjector>();
            var service = ScriptableObject.CreateInstance<ScriptableServiceA>();
            AddAssetToRegister(injector, null);
            AddAssetToRegister(injector, service);
            
            // Act
            injector.gameObject.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(injector.Injector.IsRegistered<IServiceA>(), Is.True);
            Assert.That(injector.Injector.Resolve<IServiceA>(), Is.EqualTo(service));
        }
        
        [UnityTest]
        public IEnumerator Awake_WithNonServiceAssetToRegister_ShouldPass()
        {
            // Arrange
            var injector = NewG(false).AddComponent<SGameObjectInjector>();
            var service = ScriptableObject.CreateInstance<ScriptableServiceA>();
            AddAssetToRegister(injector, NewG());
            AddAssetToRegister(injector, service);
            
            // Act
            injector.gameObject.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(injector.Injector.IsRegistered<IServiceA>(), Is.True);
            Assert.That(injector.Injector.Resolve<IServiceA>(), Is.EqualTo(service));
        }
        

        #endregion
    }
}