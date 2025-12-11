using System;

namespace interop_testing;

public class LogComponent : Component
{
    public override void Start()
    {
        Console.WriteLine("Starting Log Component");
    }

    public override void Update()
    {
        Console.WriteLine("Updating Log Component");
    }

    public override void Destroy()
    {
        Console.WriteLine("Destroying Log Component");
    }
}