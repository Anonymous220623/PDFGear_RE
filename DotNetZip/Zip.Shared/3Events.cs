// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SaveProgressEventArgs
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip;

public class SaveProgressEventArgs : ZipProgressEventArgs
{
  private int _entriesSaved;

  internal SaveProgressEventArgs(
    string archiveName,
    bool before,
    int entriesTotal,
    int entriesSaved,
    ZipEntry entry)
    : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
  {
    this.EntriesTotal = entriesTotal;
    this.CurrentEntry = entry;
    this._entriesSaved = entriesSaved;
  }

  internal SaveProgressEventArgs()
  {
  }

  internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor)
    : base(archiveName, flavor)
  {
  }

  internal static SaveProgressEventArgs ByteUpdate(
    string archiveName,
    ZipEntry entry,
    long bytesXferred,
    long totalBytes)
  {
    SaveProgressEventArgs progressEventArgs = new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead);
    progressEventArgs.ArchiveName = archiveName;
    progressEventArgs.CurrentEntry = entry;
    progressEventArgs.BytesTransferred = bytesXferred;
    progressEventArgs.TotalBytesToTransfer = totalBytes;
    return progressEventArgs;
  }

  internal static SaveProgressEventArgs Started(string archiveName)
  {
    return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
  }

  internal static SaveProgressEventArgs Completed(string archiveName)
  {
    return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
  }

  public int EntriesSaved => this._entriesSaved;
}
