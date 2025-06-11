// Decompiled with JetBrains decompiler
// Type: FileWatcher.Watcher
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace FileWatcher;

public class Watcher : IDisposable
{
  private bool disposedValue;
  private object locker = new object();
  private List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

  public bool AddPath(string path, string filter = "*.*", bool includeSubdirectories = false)
  {
    this.ThrowIfDisposed();
    if (string.IsNullOrEmpty(path))
      return false;
    lock (this.locker)
    {
      this.RemovePath(path);
      FileSystemWatcher fileSystemWatcher = (FileSystemWatcher) null;
      string[] strArray = filter.Split('|');
      if (strArray.Length == 0)
        strArray = new string[1]{ "" };
      for (int index = 0; index < strArray.Length; ++index)
      {
        try
        {
          fileSystemWatcher = new FileSystemWatcher(path, strArray[index]);
          fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
          fileSystemWatcher.Created += new FileSystemEventHandler(this.Watcher_Created);
          fileSystemWatcher.Renamed += new RenamedEventHandler(this.Watcher_Renamed);
          fileSystemWatcher.IncludeSubdirectories = includeSubdirectories;
          fileSystemWatcher.EnableRaisingEvents = true;
          this.watchers.Add(fileSystemWatcher);
          return true;
        }
        catch
        {
          fileSystemWatcher?.Dispose();
        }
      }
    }
    return false;
  }

  public void RemovePath(string path)
  {
    this.ThrowIfDisposed();
    lock (this.locker)
    {
      for (int index = this.watchers.Count - 1; index >= 0; --index)
      {
        if (this.watchers[index].Path == path)
        {
          try
          {
            this.watchers[index].Created -= new FileSystemEventHandler(this.Watcher_Created);
            this.watchers[index].Renamed -= new RenamedEventHandler(this.Watcher_Renamed);
            this.watchers[index].Dispose();
          }
          catch
          {
          }
          this.watchers.RemoveAt(index);
        }
      }
    }
  }

  public void Clear()
  {
    this.ThrowIfDisposed();
    lock (this.locker)
    {
      for (int index = 0; index < this.watchers.Count; ++index)
      {
        try
        {
          this.watchers[index].Created -= new FileSystemEventHandler(this.Watcher_Created);
          this.watchers[index].Renamed -= new RenamedEventHandler(this.Watcher_Renamed);
          this.watchers[index].Dispose();
        }
        catch
        {
        }
      }
      this.watchers.Clear();
    }
  }

  private void Watcher_Created(object sender, FileSystemEventArgs e)
  {
    WatcherFileCreatedEventHandler fileCreated = this.FileCreated;
    if (fileCreated == null)
      return;
    string path = ((FileSystemWatcher) sender).Path;
    string fullPath = e.FullPath;
    fileCreated(this, new WatcherFileCreatedEventArgs(path, fullPath));
  }

  private void Watcher_Renamed(object sender, RenamedEventArgs e)
  {
    WatcherFileRenamedEventHandler fileRenamed = this.FileRenamed;
    if (fileRenamed == null)
      return;
    string path = ((FileSystemWatcher) sender).Path;
    if (fileRenamed == null)
      return;
    fileRenamed(this, new WatcherFileRenamedEventArgs(path, e.OldFullPath, e.FullPath));
  }

  public event WatcherFileCreatedEventHandler FileCreated;

  public event WatcherFileRenamedEventHandler FileRenamed;

  private void ThrowIfDisposed()
  {
    if (this.disposedValue)
      throw new ObjectDisposedException(nameof (Watcher));
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    lock (this.locker)
    {
      for (int index = 0; index < this.watchers.Count; ++index)
      {
        try
        {
          this.watchers[index].Created -= new FileSystemEventHandler(this.Watcher_Created);
          this.watchers[index].Renamed -= new RenamedEventHandler(this.Watcher_Renamed);
          this.watchers[index].Dispose();
        }
        catch
        {
        }
      }
      this.watchers.Clear();
      this.watchers = (List<FileSystemWatcher>) null;
    }
    this.disposedValue = true;
  }

  ~Watcher() => this.Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
