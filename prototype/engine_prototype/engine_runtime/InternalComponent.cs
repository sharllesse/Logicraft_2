namespace EngineRuntime;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal static partial class InternalComponent
{
    [LibraryImport(libraryName: "EngineCore", EntryPoint = "set_global_component_callbacks", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetGlobalComponentCallback(
        delegate* unmanaged[Cdecl]<IntPtr, void> startPtr, 
        delegate* unmanaged[Cdecl]<IntPtr, void> updatePtr,
        delegate* unmanaged[Cdecl]<IntPtr, void> destroyPtr);
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    internal static void ComponentStart(IntPtr gcHandlePtr)
    {
        GCHandle gcHandle = GCHandle.FromIntPtr(gcHandlePtr);
        Component? component = (Component?)gcHandle.Target;
        component?.Start();
    }
    
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    internal static void ComponentUpdate(IntPtr gcHandlePtr)
    {
        GCHandle gcHandle = GCHandle.FromIntPtr(gcHandlePtr);
        Component? component = (Component?)gcHandle.Target;
        component?.Update();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    internal static void ComponentDestroy(IntPtr gcHandlePtr)
    {
        GCHandle gcHandle = GCHandle.FromIntPtr(gcHandlePtr);
        Component? component = ((Component?)gcHandle.Target);
        component?.Destroy();
        gcHandle.Free();
    }
}