using System;

namespace EngineRuntime;

public class Component
{
    internal IntPtr InternalReference;
    
    public virtual void Start() {}
    
    public virtual void Update() {}
    
    public virtual void Destroy() {}
}