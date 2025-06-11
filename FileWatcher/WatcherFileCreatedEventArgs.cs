// Decompiled with JetBrains decompiler
// Type: FileWatcher.WatcherFileCreatedEventArgs
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

#nullable disable
namespace FileWatcher;

public class WatcherFileCreatedEventArgs
{
  public WatcherFileCreatedEventArgs(string watchingPath, string createdFile)
  {
    this.WatchingPath = watchingPath;
    this.CreatedFile = createdFile;
  }

  public string WatchingPath { get; }

  public string CreatedFile { get; }
}
