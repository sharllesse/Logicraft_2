using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Editor;

public sealed class ScriptReloader : IDisposable
{
    private struct ModifiedFileData(string fullPath, string name, string extension)
    {
        public ModifiedFileData(
            string fullPath, 
            string name, 
            string extension, 
            string oldFullPath, 
            string oldName,
            string oldExtension) : this(fullPath, name, extension)
        {
            OldFullPath = oldFullPath;
            OldName = oldName;
            OldExtension = oldExtension;
        }
        
        public string FullPath = fullPath;
        public string Name = name;
        public string Extension = extension;
        public string? OldFullPath;
        public string? OldName;
        public string? OldExtension;
    }
    
    private const string ScriptAssemblyName = "GameScript";
    private static readonly string ScriptAssemblyPath = $"{AppDomain.CurrentDomain.BaseDirectory}/{ScriptAssemblyName}.dll";
    
    //Temporary
    private static readonly string ScriptAssemblyProjPath =
        $"{AppDomain.CurrentDomain.BaseDirectory}../../../game_script/{ScriptAssemblyName}.csproj";
    
#if DEBUG
    private const string CurrentConfig = "Debug";
#else
    private const string CurrentConfig = "Release";
#endif

    private static readonly string ScriptAssemblyArtefactPath =
        $"{AppDomain.CurrentDomain.BaseDirectory}/../../../../../artefacts/managed/{CurrentConfig}/{ScriptAssemblyName}/{ScriptAssemblyName}.dll";
    
    private Assembly? _scriptAssembly;
    private DynamicAssemblyLoadContext? _assemblyLoadContext;

    private readonly object _modifiedAssemblyVarLock = new();

    private Process? _buildScriptAssemblyProcess;

    public delegate void LoadScriptAssemblyEventHandler(in Assembly scriptAssembly);
    public LoadScriptAssemblyEventHandler? OnLoadScriptAssembly;
    
    public delegate void UnloadScriptAssemblyEventHandler(in Assembly scriptAssembly);
    public UnloadScriptAssemblyEventHandler? OnUnloadScriptAssembly;

    //private readonly Timer _fileModifiedTimer = new(TimeSpan.FromMilliseconds(500d));
    //private readonly List<ModifiedFileData> _modifiedFileDatas = [];

    //For now the hot reload will work with one file per one file.
    //Later I will do it with multiple files. But I need to use some wierd thing I don't know anything about. 
    private bool _isLoadingAssembly;
    
    public ScriptReloader(AssetsFileWatcher assetsFileWatcher)
    {
        assetsFileWatcher.OnChanged += OnAnyFileChanged;
        assetsFileWatcher.OnCreated += OnAnyFileCreated;
        assetsFileWatcher.OnDeleted += OnAnyFileDeleted;
        assetsFileWatcher.OnRenamed += OnAnyFileRenamed;

        //_fileModifiedTimer.AutoReset = false;
        //_fileModifiedTimer.Elapsed += OnFileModifiedTimerElapsed;
    }

    private void OnAnyFileChanged(object sender, FileSystemEventArgs e)
    {
        string? changedFileExtension = Path.GetExtension(e.FullPath);
        if (changedFileExtension is not ".cs")
        {
            return;
        }

        string? changedFileName = Path.GetFileNameWithoutExtension(e.FullPath);
        Console.WriteLine($"The file {changedFileName} has changed.");

        //ModifiedFileData modifiedFileData = new ModifiedFileData(e.FullPath, changedFileName, changedFileExtension);
        //RegisterFile(in modifiedFileData);
        LoadScriptAssembly();
    }

    private void OnAnyFileCreated(object sender, FileSystemEventArgs e)
    {
        ReadOnlySpan<char> changedFileExtension = Path.GetExtension(e.FullPath);
        if (changedFileExtension is not ".cs")
        {
            return;
        }

        ReadOnlySpan<char> changedFile = Path.GetFileName(e.FullPath);
        Console.WriteLine($"The file {changedFile} has created.");
        
        //LoadScriptAssembly();
    }
    
    private void OnAnyFileDeleted(object sender, FileSystemEventArgs e)
    {
        ReadOnlySpan<char> changedFileExtension = Path.GetExtension(e.FullPath);
        if (changedFileExtension is not ".cs")
        {
            return;
        }

        ReadOnlySpan<char> changedFile = Path.GetFileName(e.FullPath);
        Console.WriteLine($"The file {changedFile} has deleted.");
        
        //LoadScriptAssembly();
    }
    
    private void OnAnyFileRenamed(object sender, RenamedEventArgs e)
    {
        ReadOnlySpan<char> oldRenamedFileExtension = Path.GetExtension(e.OldFullPath);
        ReadOnlySpan<char> oldRenamedFileName = Path.GetFileName(e.OldName);
        if (oldRenamedFileExtension is ".cs")
        {
            Console.WriteLine($"Deleting the meta data of {oldRenamedFileName}.");
        }
        
        ReadOnlySpan<char> renamedFileExtension = Path.GetExtension(e.FullPath);
        if (renamedFileExtension is not ".cs")
        {
            return;
        }

        ReadOnlySpan<char> changedFile = Path.GetFileName(e.FullPath);
        Console.WriteLine($"The file {changedFile} has deleted.");
        
        //LoadScriptAssembly();
    }

    // private void RegisterFile(in ModifiedFileData modifiedFileData)
    // {
    //     _fileModifiedTimer.Stop();
    //     _modifiedFileDatas.Add(modifiedFileData);
    //     _fileModifiedTimer.Start();
    // }
    //
    // private void OnFileModifiedTimerElapsed(object? o, ElapsedEventArgs args)
    // {
    //     _fileModifiedTimer.Enabled
    //     Console.WriteLine($"File modified in this thread ! {Thread.CurrentThread}");
    //     Thread.Sleep(TimeSpan.FromSeconds(10d));
    //     Console.WriteLine($"File finish modified in this thread ! {Thread.CurrentThread}");
    // }

    public void LoadScriptAssembly()
    {
        if (_isLoadingAssembly)
        {
            return;
        }
        
        _isLoadingAssembly = true;
        
        //Clean of all the current game object and component.
        
        //Unload of the assembly if it currently loaded.
        //If true that mean it need to load the assembly after the old one has been unloaded.
        //So to do that a call back is used.
        UnloadScriptAssembly();

        //Then recompile of the assembly.
        ProcessStartInfo processStartInfo = new("dotnet.exe", $"build {ScriptAssemblyProjPath}");
        _buildScriptAssemblyProcess = Process.Start(processStartInfo);
        
        if (_buildScriptAssemblyProcess is null)
        {
            throw new ApplicationException("The script assembly process was null.");
        }

        _buildScriptAssemblyProcess.EnableRaisingEvents = true;

        if (_buildScriptAssemblyProcess.HasExited)
        {
            Console.WriteLine("Script Assembly has been build with success and has directly Exited.");
            OnScriptAssemblyBuildFinish(this, EventArgs.Empty);
            return;
        }
        
        Console.WriteLine("Script Assembly build process has been launched");
        _buildScriptAssemblyProcess.Exited += OnScriptAssemblyBuildFinish;
    }

    private void OnScriptAssemblyBuildFinish(object? s, EventArgs eventArgs)
    {
        Console.WriteLine("Script Assembly has been build with success and has successfully copied the Script assembly files.");
        
        lock (_modifiedAssemblyVarLock)
        {
            //Load of the assembly.
            File.Copy(ScriptAssemblyArtefactPath, ScriptAssemblyPath, overwrite: true);
            
            (_assemblyLoadContext, _scriptAssembly) = DynamicAssemblyLoadContext.LoadDynamicAssembly(ScriptAssemblyPath);

            if (_scriptAssembly is not null)
            {
                OnLoadScriptAssembly?.Invoke(in _scriptAssembly);   
            }
        }
        
        _isLoadingAssembly = false;
    }
    
    public void UnloadScriptAssembly()
    {
        lock (_modifiedAssemblyVarLock)
        {
            if (_scriptAssembly is not null)
            {
                OnUnloadScriptAssembly?.Invoke(in _scriptAssembly);   
            }
            
            _assemblyLoadContext?.Unload();
            _assemblyLoadContext = null;
            _scriptAssembly = null;
        }
    }

    public void Dispose()
    {
        UnloadScriptAssembly();
    }

    public Type? GetComponentType(string componentTypeName)
    {
        return _scriptAssembly?.GetType(componentTypeName);
    }
}