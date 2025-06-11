// Decompiled with JetBrains decompiler
// Type: NLog.Internal.MultiFileWatcher
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NLog.Internal;

internal sealed class MultiFileWatcher : IDisposable
{
  private readonly Dictionary<string, FileSystemWatcher> _watcherMap = new Dictionary<string, FileSystemWatcher>();

  public NotifyFilters NotifyFilters { get; set; }

  public event FileSystemEventHandler FileChanged;

  public MultiFileWatcher()
    : this(NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Security)
  {
  }

  public MultiFileWatcher(NotifyFilters notifyFilters) => this.NotifyFilters = notifyFilters;

  public void Dispose()
  {
    this.FileChanged = (FileSystemEventHandler) null;
    this.StopWatching();
    GC.SuppressFinalize((object) this);
  }

  public void StopWatching()
  {
    lock (this._watcherMap)
    {
      foreach (FileSystemWatcher watcher in this._watcherMap.Values)
        this.StopWatching(watcher);
      this._watcherMap.Clear();
    }
  }

  public void StopWatching(string fileName)
  {
    lock (this._watcherMap)
    {
      FileSystemWatcher watcher;
      if (!this._watcherMap.TryGetValue(fileName, out watcher))
        return;
      this.StopWatching(watcher);
      this._watcherMap.Remove(fileName);
    }
  }

  public void Watch(IEnumerable<string> fileNames)
  {
    if (fileNames == null)
      return;
    foreach (string fileName in fileNames)
      this.Watch(fileName);
  }

  internal void Watch(string fileName)
  {
    string directoryName = Path.GetDirectoryName(fileName);
    if (!Directory.Exists(directoryName))
    {
      InternalLogger.Warn<string, string>("Cannot watch file {0} for changes as directory {1} doesn't exist", fileName, directoryName);
    }
    else
    {
      string fileName1 = Path.GetFileName(fileName);
      lock (this._watcherMap)
      {
        if (this._watcherMap.ContainsKey(fileName))
          return;
        FileSystemWatcher watcher = (FileSystemWatcher) null;
        try
        {
          watcher = new FileSystemWatcher()
          {
            Path = directoryName,
            Filter = fileName1,
            NotifyFilter = this.NotifyFilters
          };
          watcher.Created += new FileSystemEventHandler(this.OnFileChanged);
          watcher.Changed += new FileSystemEventHandler(this.OnFileChanged);
          watcher.Deleted += new FileSystemEventHandler(this.OnFileChanged);
          watcher.Renamed += new RenamedEventHandler(this.OnFileChanged);
          watcher.Error += new ErrorEventHandler(this.OnWatcherError);
          watcher.EnableRaisingEvents = true;
          InternalLogger.Debug<string, string>("Watching path '{0}' filter '{1}' for changes.", watcher.Path, watcher.Filter);
          this._watcherMap.Add(fileName, watcher);
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "Failed Watching path '{0}' with file '{1}' for changes.", (object) directoryName, (object) fileName);
          if (ex.MustBeRethrown())
            throw;
          if (watcher == null)
            return;
          this.StopWatching(watcher);
        }
      }
    }
  }

  private void StopWatching(FileSystemWatcher watcher)
  {
    try
    {
      InternalLogger.Debug<string, string>("Stopping file watching for path '{0}' filter '{1}'", watcher.Path, watcher.Filter);
      watcher.EnableRaisingEvents = false;
      watcher.Created -= new FileSystemEventHandler(this.OnFileChanged);
      watcher.Changed -= new FileSystemEventHandler(this.OnFileChanged);
      watcher.Deleted -= new FileSystemEventHandler(this.OnFileChanged);
      watcher.Renamed -= new RenamedEventHandler(this.OnFileChanged);
      watcher.Error -= new ErrorEventHandler(this.OnWatcherError);
      watcher.Dispose();
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Failed to stop file watcher for path '{0}' filter '{1}'", (object) watcher.Path, (object) watcher.Filter);
      if (!ex.MustBeRethrown())
        return;
      throw;
    }
  }

  private void OnWatcherError(object source, ErrorEventArgs e)
  {
    string str = string.Empty;
    if (source is FileSystemWatcher fileSystemWatcher)
      str = fileSystemWatcher.Path;
    Exception exception = e.GetException();
    if (exception != null)
      InternalLogger.Warn(exception, "Error Watching Path {0}", (object) str);
    else
      InternalLogger.Warn<string>("Error Watching Path {0}", str);
  }

  private void OnFileChanged(object source, FileSystemEventArgs e)
  {
    FileSystemEventHandler fileChanged = this.FileChanged;
    if (fileChanged == null)
      return;
    try
    {
      fileChanged(source, e);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Error Handling File Changed");
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
  }
}
