using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace interop_testing;

internal static partial class ExternalComponent
{
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "set_global_component_callbacks", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static unsafe partial void SetGlobalComponentCallback(
        delegate* unmanaged[Cdecl]<IntPtr, void> startPtr, 
        delegate* unmanaged[Cdecl]<IntPtr, void> updatePtr,
        delegate* unmanaged[Cdecl]<IntPtr, void> destroyPtr);
    
    [UnmanagedCallersOnly(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static void ComponentStart(IntPtr gcHandlePtr)
    {
        GCHandle gcHandle = GCHandle.FromIntPtr(gcHandlePtr);
        Component component = (Component)gcHandle.Target;
        component?.Start();
    }
    
    [UnmanagedCallersOnly(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static void ComponentUpdate(IntPtr gcHandlePtr)
    {
        GCHandle gcHandle = GCHandle.FromIntPtr(gcHandlePtr);
        Component component = (Component)gcHandle.Target;
        component?.Update();
    }

    [UnmanagedCallersOnly(CallConvs = new []{typeof(CallConvCdecl)})]
    internal static void ComponentDestroy(IntPtr gcHandlePtr)
    {
        GCHandle gcHandle = GCHandle.FromIntPtr(gcHandlePtr);
        ((Component)gcHandle.Target)?.Destroy();
        gcHandle.Free();
    }
}

public class Component
{
    internal IntPtr InternalReference;

    public virtual void Start() {}
    
    public virtual void Update() {}
    
    public virtual void Destroy() {}
}