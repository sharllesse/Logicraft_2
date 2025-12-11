using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace interop_testing;

internal static partial class ExternalVector3
{
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "add_vector")]
    [UnmanagedCallConv(CallConvs = new []{ typeof(CallConvCdecl)})]
    internal static partial Vector3 AddVector(in Vector3 first, in Vector3 second);
    
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "vector_dot_product")]
    [UnmanagedCallConv(CallConvs = new []{ typeof(CallConvCdecl)})]
    internal static partial float VectorDotProduct(in Vector3 first, in Vector3 second);
    
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "vector_length")]
    [UnmanagedCallConv(CallConvs = new []{ typeof(CallConvCdecl)})]
    internal static partial float VectorLength(in Vector3 vector);
    
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "vector_normalized")]
    [UnmanagedCallConv(CallConvs = new []{ typeof(CallConvCdecl)})]
    internal static partial Vector3 VectorNormalized(in Vector3 vector);
    
    [LibraryImport(libraryName: "InteropCPP", EntryPoint = "vector_to_identity")]
    [UnmanagedCallConv(CallConvs = new []{ typeof(CallConvCdecl)})]
    internal static partial Quaternion VectorToIdentity(in Vector3 vector);
}

[StructLayout(LayoutKind.Sequential)]
public struct Vector3(float x, float y, float z)
{
    public static Vector3 operator+(Vector3 first, Vector3 second)
    {
        return ExternalVector3.AddVector(in first, in second);
    }

    public float DotProduct(in Vector3 other)
    {
        return ExternalVector3.VectorDotProduct(in this, in other);
    }
    
    public float Length()
    {
        return ExternalVector3.VectorLength(in this);
    }
    
    public Vector3 Normalized()
    {
        return ExternalVector3.VectorNormalized(in this);
    }
    
    public Quaternion ToIdentity()
    {
        return ExternalVector3.VectorToIdentity(in this);
    }

    public override string ToString()
    {
        return $"x : {x}, y : {y}, z : {z}";
    }

    public float x = x, y = y, z = z;
}