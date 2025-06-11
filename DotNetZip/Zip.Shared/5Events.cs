// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipErrorEventArgs
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;

#nullable disable
namespace Ionic.Zip;

public class ZipErrorEventArgs : ZipProgressEventArgs
{
  private Exception _exc;

  private ZipErrorEventArgs()
  {
  }

  internal static ZipErrorEventArgs Saving(string archiveName, ZipEntry entry, Exception exception)
  {
    ZipErrorEventArgs zipErrorEventArgs = new ZipErrorEventArgs();
    zipErrorEventArgs.EventType = ZipProgressEventType.Error_Saving;
    zipErrorEventArgs.ArchiveName = archiveName;
    zipErrorEventArgs.CurrentEntry = entry;
    zipErrorEventArgs._exc = exception;
    return zipErrorEventArgs;
  }

  public Exception Exception => this._exc;

  public string FileName => this.CurrentEntry.LocalFileName;
}
