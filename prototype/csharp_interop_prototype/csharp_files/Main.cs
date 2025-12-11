using System;
using System.Runtime.InteropServices;

namespace interop_testing;

public class InteropTesting
{
    public static void Main(string[] args)
    {
        /*int result = External.Add(5, 10);
        Console.WriteLine($"5 + 10 = {result}");

        Vector3 firstVector = new (20, 20, 20);
        Vector3 secondVector = new (20, 20, 20);
        
        Console.WriteLine($"FirstVector : {firstVector} + secondVector : {secondVector} = {firstVector + secondVector}");
        
        Console.WriteLine($"Dot product of the first and second vector : {firstVector.Normalized().DotProduct(secondVector.Normalized())}");
        
        Console.WriteLine($"Length of the first vector : {firstVector.Length()}");
        
        Console.WriteLine($"Normalized first vector : {firstVector.Normalized()}");
        
        Console.WriteLine($"Identity of the first vector : {firstVector.Normalized().ToIdentity()}");

        Vector3 vector = new Vector3(10f, 20f, 30f);
        IntPtr pVector = Marshal.AllocHGlobal(Marshal.SizeOf<Vector3>());
        Marshal.StructureToPtr(vector, pVector, false);

        float x = Marshal.PtrToStructure<float>(pVector);
        
        Marshal.StructureToPtr(x, pVector + 4, false);
        
        vector = Marshal.PtrToStructure<Vector3>(pVector);

        Console.WriteLine(vector);
        
        Marshal.FreeHGlobal(pVector);*/

        unsafe
        {
            ExternalComponent.SetGlobalComponentCallback(
                &ExternalComponent.ComponentStart, 
                &ExternalComponent.ComponentUpdate, 
                &ExternalComponent.ComponentDestroy);
        }

        GameObject gameObject = new GameObject("super game object");
        gameObject.AddComponent<LogComponent>();
        
        ExternalGameObject.UpdateGameObjects();
    }
}