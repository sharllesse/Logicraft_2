namespace EngineRuntime;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal static partial class InternalGameObject
{
    [LibraryImport(libraryName: "EngineCore", EntryPoint = "create_game_object", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr CreateGameObject(string name, int nameSize);

    [LibraryImport(libraryName: "EngineCore", EntryPoint = "add_component")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr AddComponent(IntPtr internalGameObject, IntPtr gcHandle);
    
    internal static T AddComponent<T>(IntPtr internalGoReference) where T : Component, new()
    {
        return (T)AddComponent(typeof(T), internalGoReference);
    }

    internal static Component AddComponent(Type componentType, IntPtr internalGoReference)
    {
        if (componentType.BaseType != typeof(Component))
        {
            throw new Exception();
            // Handle Error.
        }

        Component? newComponent = (Component?)Activator.CreateInstance(componentType);
        if (newComponent is null)
        {
            throw new Exception();
            //Handle Error.
        }

        var gcHandle = GCHandle.Alloc(newComponent, GCHandleType.Normal);
        newComponent.InternalReference = AddComponent(internalGoReference, GCHandle.ToIntPtr(gcHandle));
        return newComponent;
    }
}