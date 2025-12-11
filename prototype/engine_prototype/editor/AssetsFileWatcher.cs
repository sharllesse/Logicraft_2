using System;
using System.IO;

namespace Editor;

public sealed class AssetsFileWatcher(string assetPath) : IDisposable
{
    private readonly FileSystemWatcher _systemWatcher = new(assetPath);
    
    public string AssetPath => assetPath;

    public event FileSystemEventHandler OnChanged
    {
        add => _systemWatcher.Changed += value;
        remove => _systemWatcher.Changed -= value;
    }
    
    public event FileSystemEventHandler OnCreated
    {
        add => _systemWatcher.Created += value;
        remove => _systemWatcher.Created -= value;
    }
    
    public event FileSystemEventHandler OnDeleted
    {
        add => _systemWatcher.Deleted += value;
        remove => _systemWatcher.Deleted -= value;
    }
    
    public event RenamedEventHandler OnRenamed
    {
        add => _systemWatcher.Renamed += value;
        remove => _systemWatcher.Renamed -= value;
    }

    public void Start()
    {
        _systemWatcher.NotifyFilter = NotifyFilters.Attributes
                                      | NotifyFilters.DirectoryName
                                      | NotifyFilters.FileName
                                      | NotifyFilters.LastWrite;

        _systemWatcher.Filter = "*.cs";
        _systemWatcher.IncludeSubdirectories = true;
        
        _systemWatcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
        _systemWatcher.EnableRaisingEvents = false;
    }

    public void Dispose()
    {
        Stop();
        _systemWatcher.Dispose();
    }
}