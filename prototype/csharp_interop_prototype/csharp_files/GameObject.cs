using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace interop_testing;

internal static partial class ExternalGameObject
{
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "create_game_object", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static partial IntPtr CreateGameObject(string name, int nameSize);
    
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "update_game_objects")]
    [UnmanagedCallConv(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static partial void UpdateGameObjects();
    
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "add_component")]
    [UnmanagedCallConv(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static partial IntPtr AddComponent(IntPtr internalGameObject, IntPtr gcHandle);
}

public sealed class GameObject
{
    private readonly IntPtr _internalReference;

    public GameObject()
    {
        _internalReference = ExternalGameObject.CreateGameObject("GameObject", "GameObject".Length);
    }
    
    public GameObject(string name)
    {
        _internalReference = ExternalGameObject.CreateGameObject(name, name.Length);
    }

    public T AddComponent<T>() where T : Component, new()
    {
        var component = new T();
        var gcHandle = GCHandle.Alloc(component, GCHandleType.Normal);
        component.InternalReference = ExternalGameObject.AddComponent(_internalReference, GCHandle.ToIntPtr(gcHandle));
        return component;
    }
}