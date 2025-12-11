using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EngineRuntime;

public sealed class GameObject
{
    private readonly IntPtr _internalReference;
    private const string GameObjectDefaultName = "GameObject";

    public GameObject()
    {
        _internalReference = InternalGameObject.CreateGameObject(GameObjectDefaultName, GameObjectDefaultName.Length);
    }
    
    public GameObject(string name)
    {
        _internalReference = InternalGameObject.CreateGameObject(name, name.Length);
    }

    public T AddComponent<T>() where T : Component, new()
    {
        return InternalGameObject.AddComponent<T>(_internalReference);
    }

    public Component AddComponent(Type componentType)
    {
        return InternalGameObject.AddComponent(componentType, _internalReference);
    }
}