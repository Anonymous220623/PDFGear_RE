// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventArgs
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;

#nullable disable
namespace Ionic.Zip;

public class ZipProgressEventArgs : EventArgs
{
  private int _entriesTotal;
  private bool _cancel;
  private ZipEntry _latestEntry;
  private ZipProgressEventType _flavor;
  private string _archiveName;
  private long _bytesTransferred;
  private long _totalBytesToTransfer;

  internal ZipProgressEventArgs()
  {
  }

  internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
  {
    this._archiveName = archiveName;
    this._flavor = flavor;
  }

  public int EntriesTotal
  {
    get => this._entriesTotal;
    set => this._entriesTotal = value;
  }

  public ZipEntry CurrentEntry
  {
    get => this._latestEntry;
    set => this._latestEntry = value;
  }

  public bool Cancel
  {
    get => this._cancel;
    set => this._cancel |= value;
  }

  public ZipProgressEventType EventType
  {
    get => this._flavor;
    set => this._flavor = value;
  }

  public string ArchiveName
  {
    get => this._archiveName;
    set => this._archiveName = value;
  }

  public long BytesTransferred
  {
    get => this._bytesTransferred;
    set => this._bytesTransferred = value;
  }

  public long TotalBytesToTransfer
  {
    get => this._totalBytesToTransfer;
    set => this._totalBytesToTransfer = value;
  }
}
