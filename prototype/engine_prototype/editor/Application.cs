using System;
using System.IO;
using System.Reflection;
using EngineRuntime;

namespace Editor;

public sealed class Application : IDisposable
{
    private readonly AssetsFileWatcher? _assetsFileWatcher;
    private readonly ScriptReloader? _scriptReloader;

    public static readonly string DataPath = $"{AppDomain.CurrentDomain.BaseDirectory}../../Assets";
    
    //Temporary
    private GameObject? _gameObject;
    
    public Application()
    {
        if (Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);   
        }
        
        GeneralCall.InitComponentCallback();
        
        _assetsFileWatcher = new AssetsFileWatcher(DataPath);
        _assetsFileWatcher.Start();

        _scriptReloader = new ScriptReloader(_assetsFileWatcher);
        _scriptReloader.OnLoadScriptAssembly += OnLoadScriptAssembly;
        _scriptReloader.OnUnloadScriptAssembly += OnUnloadScriptAssembly;
        _scriptReloader.LoadScriptAssembly();
    }
    
    public void Run()
    {
        while (true)
        {
            if (WantToExit())
            {
                break;
            }
            
            //I need to stop the update when the reload of the game script dll is done.
            //I need to put thread safety in the native dll.
            //I need to find a proper way to stock the linked lib to avoid the Assembly crash.
            
            GeneralCall.UpdateAllGameObject();
        }
    }

    private bool WantToExit()
    {
        if (!Console.KeyAvailable)
        {
            return false;
        }
        
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
        return consoleKeyInfo.Key == ConsoleKey.Escape;
    }

    private void OnLoadScriptAssembly(in Assembly scriptAssembly)
    {
        Type? type = scriptAssembly.GetType("LogComponent");

        if (type is null)
        {
            Console.WriteLine("Unable to find type LogComponent");
            return;
        }
        
        _gameObject = new GameObject();
        _gameObject.AddComponent(type);
    }
    
    private void OnUnloadScriptAssembly(in Assembly scriptAssembly)
    {
        //For now i don't need to stop the update since i put mutex to avoid destroying and updating at the same time.
        //Later it would be better to stop any update (When a play and editor mode are made.).
        GeneralCall.DestroyAllGameObject();
    }
    
    public void Dispose()
    {
        _assetsFileWatcher?.Dispose();
        _scriptReloader?.Dispose();
    }
}