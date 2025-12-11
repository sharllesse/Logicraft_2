using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EngineRuntime;

internal static partial class InternalGeneralCall
{
    [LibraryImport(libraryName: "EngineCore", EntryPoint = "update_all_game_object")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void UpdateAllGameObject();
    
    [LibraryImport(libraryName: "EngineCore", EntryPoint = "destroy_all_game_object")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void DestroyAllGameObject();
}