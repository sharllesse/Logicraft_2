using System.Runtime.InteropServices;

namespace interop_testing;

[StructLayout(LayoutKind.Sequential)]
public struct Quaternion(float x, float y, float z, float w)
{
    public override string ToString()
    {
        return $"x : {x}, y : {y}, z : {z}, w : {w}";
    }
    
    public float x = x, y = y, z = z, w = w;
}