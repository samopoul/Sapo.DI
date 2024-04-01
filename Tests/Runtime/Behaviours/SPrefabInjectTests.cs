using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sapo.DI.Runtime.Behaviours;
using Sapo.DI.Tests.Runtime.Helpers;
using Sapo.DI.Tests.Runtime.TestData;
using UnityEngine;
using UnityEngine.TestTools;
using Is = Sapo.DI.Tests.Runtime.Helpers.Is;

namespace Sapo.DI.Tests.Runtime.Behaviours
{
    [TestFixture]
    internal class SPrefabInjectTests
    {
        private readonly List<GameObject> _gameObjects = new();
        
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
        public IEnumerator Awake_WithNoRootInjector_ShouldDestroySelf()
        {
            // Arrange
            LogAssert.ignoreFailingMessages = true;
            var g = NewG(false);
            var inject = g.AddComponent<SPrefabInject>();
            
            // Act
            g.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(inject, Is.Destroyed);
            Assert.That(g, Is.Not.Destroyed());
        }

        [UnityTest]
        public IEnumerator Awake_WithRootInjector_ShouldInjectAndDestroySelf()
        {
            // Arrange
            var injector = NewG().AddComponent<SRootInjector>();
            var g = NewG(false);
            var inject = g.AddComponent<SPrefabInject>();
            var service = g.AddComponent<ComponentServiceA>();
            
            // Act
            g.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(inject, Is.Destroyed);
            Assert.That(g, Is.Not.Destroyed());
            Assert.That(injector.Injector.IsRegistered<IServiceA>(), Is.True);
            Assert.That(injector.Injector.Resolve<IServiceA>(), Is.EqualTo(service));
        }

        [UnityTest]
        public IEnumerator Awake_WithGameObjectInjector_ShouldInjectAndDestroySelf()
        {
            // Arrange
            var g = NewG(false);
            var injector = g.AddComponent<SGameObjectInjector>();
            var inject = g.AddComponent<SPrefabInject>();
            var service = g.AddComponent<ComponentServiceA>();
            var component = g.AddComponent<ComponentWithDependencyToA>();

            // Act
            g.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(inject, Is.Destroyed);
            Assert.That(g, Is.Not.Destroyed());
            Assert.That(injector.Injector.IsRegistered<IServiceA>(), Is.True);
            Assert.That(injector.Injector.Resolve<IServiceA>(), Is.EqualTo(service));
            Assert.That(component.ServiceA, Is.EqualTo(service));
        }

        [UnityTest]
        public IEnumerator Awake_WithGameObjectInjector_ShouldCallEvents_WithCorrectOrder()
        {
            // Arrange
            var g = NewG(false);
            var injector = g.AddComponent<SGameObjectInjector>();
            var inject = g.AddComponent<SPrefabInject>();
            var component = g.AddComponent<ComponentWithEvents>();
            
            // Act
            g.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(inject, Is.Destroyed);
            Assert.That(component.Events, Is.EqualTo(new[] { "OnRegister", "OnInject" }));
        }
        
        [UnityTest]
        public IEnumerator Awake_WithAlreadyInjected_ShouldDestroySelf()
        {
            // Arrange
            var g = NewG(false);
            var injector = g.AddComponent<SGameObjectInjector>();
            var inject = g.AddComponent<SPrefabInject>();
            injector.Inject();

            // Act
            g.SetActive(true);
            yield return null;
            
            // Assert
            Assert.That(inject, Is.Destroyed);
            Assert.That(g, Is.Not.Destroyed());
        }

        #endregion
        
    }
}