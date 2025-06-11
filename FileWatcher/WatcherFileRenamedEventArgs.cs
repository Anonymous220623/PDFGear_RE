// Decompiled with JetBrains decompiler
// Type: FileWatcher.WatcherFileRenamedEventArgs
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

#nullable disable
namespace FileWatcher;

public class WatcherFileRenamedEventArgs
{
  public WatcherFileRenamedEventArgs(string watchingPath, string oldFileName, string newFileName)
  {
    this.WatchingPath = watchingPath;
    this.OldFileName = oldFileName;
    this.NewFileName = newFileName;
  }

  public string WatchingPath { get; }

  public string OldFileName { get; }

  public string NewFileName { get; }
}
