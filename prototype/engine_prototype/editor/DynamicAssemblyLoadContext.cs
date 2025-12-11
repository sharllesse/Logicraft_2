using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Editor;

public class DynamicAssemblyLoadContext(string assemblyPath) : AssemblyLoadContext(isCollectible: true)
{
    private AssemblyDependencyResolver _resolver = new AssemblyDependencyResolver(assemblyPath);
    
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name == "EngineRuntime")
        {
            return null;
        }
        
        string? assembly = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assembly is null)
        {
            return null;
        }

        using var fs = new FileStream(assembly, FileMode.Open, FileAccess.Read);
        return LoadFromStream(fs);
    }

    public static (DynamicAssemblyLoadContext assemblyLoadContext, Assembly loadedAssembly) LoadDynamicAssembly(string assemblyPath)
    {
        DynamicAssemblyLoadContext assemblyLoadContext = new DynamicAssemblyLoadContext(assemblyPath);

        string? assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
        if (assemblyName == null)
        {
            throw new ArgumentException(
                $"DynamicAssemblyLoadContext: The given assembly path [{assemblyPath}] is invalid !");
        }

        return (assemblyLoadContext, assemblyLoadContext.LoadFromAssemblyName(new AssemblyName(assemblyName)));
    }
}