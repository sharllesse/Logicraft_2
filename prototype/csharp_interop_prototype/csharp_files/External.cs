using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace interop_testing;

internal partial class External
{
    //A simple function with a catch can be added to avoid dll exception.
    [LibraryImport(libraryName: "libInteropCPP", EntryPoint = "add")]
    [UnmanagedCallConv(CallConvs = new []{ typeof(CallConvCdecl) })]
    internal static partial int Add(int a, int b);
}