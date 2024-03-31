[![Asset Store](https://img.shields.io/badge/Unity-2019.4.0f1-blue.svg)](https://assetstore.unity.com/publishers/99093)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

# SInject
SInject is a simple dependency injection library for unity.

## Features
- <span style="color:green">&#10003;</span> Simple and easy to use
- <span style="color:green">&#10003;</span> Supports DI for scenes and prefabs
- <span style="color:green">&#10003;</span> Allows for persistent objects across scenes
- <span style="color:red">&#10007;</span> Support for multiple root injectors
- <span style="color:green">&#10003;</span> Support for multiple scenes

## How to use

----

### Setup root injector

In order to use SInject, we need to create a root injector.
This is the main injector that will be used to inject dependencies into our objects.
In our first scene, right click in the hierarchy and select `DI/Root Injector`.
> If you don't see the `DI/Root Injector` option, it means that root injector is already set up in the scene.

Now we will see in inspector a Root Injector component.
Root injector is a singleton, so we can only have **one in scene or one across scenes**.
![Root Injector Image](Documentation~/root-injector.png)

As you can see, there are two sections (`Settings` and `Runtime info`), for now we will focus on the settings section.
There are two options:
1. `Make Persistent` - If this is checked, the root injector will not be destroyed when loading a new scene.
2. `Assets to register` - Here we can drag and drop scriptable objects that we want to register in the injector.
   Most of the time this will be your singleton services, their lifetime will be the same as the root injector's. Please note
   that you can only drag and drop scriptable objects that are defined with `SRegister` attribute.


Now after we have created the root injector, we also need to create `SceneInject` to mark this scene to be injected when loaded.
Right click in the hierarchy and select `DI/Scene Inject`.
> If you don't see the `DI/Scene Inject` option, it means that scene inject is already set up in the scene.

`Scene Inject` is a component that injects entire scene when scene is being loaded. Please note that after scene is loaded, 
gameObject on which `Scene Inject` is attached will be destroyed.

### Setup a scriptable object service

In order to inject a scriptable object, we need to define it with `SRegister` attribute.
```csharp
using Sapo.SInject.Runtime.Attributes;

[CreateAssetMenu(menuName = "ServiceA")]
[SRegister(typeof(IServiceA))] // This is the interface that we want to inject, we can also use the concrete class
public class ServiceA : ScriptableObject, IServiceA
{
    public void Introduce()
    {
        Debug.Log("Hello, I am ServiceA!");
    }
}
```
Now in unity, create this scriptable object and drag it to the `Assets to register` field in the root injector.

[![Root Injector Image](Documentation~/service-a.png)](Documentation~/service-a.png)

### Injecting dependencies

With the setup of our root injector and the registration of our scriptable object services, we are now equipped 
to inject our dependencies into various components or other scriptable objects. This allows us to utilize 
the power of dependency injection, promoting loose coupling and enhancing the modularity and testability of our code.

We can now inject dependencies into components or other scriptable objects.
```csharp
using Sapo.SInject.Runtime.Attributes;

public class Player : MonoBehaviour
{
    [Inject] private IServiceA _serviceA;

    private void Awake()
    {
        _serviceA.Introduce();
    }
}
```



