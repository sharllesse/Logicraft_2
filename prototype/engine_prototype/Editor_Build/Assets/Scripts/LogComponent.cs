using System;
using EngineRuntime;

public class LogComponent : Component
{
    public override void Start()
    {
        Console.Write("Starting Logging Component\n");
    }

    public override void Update()
    {
        Console.Write("Updating Logging Component\n");
    }
}

