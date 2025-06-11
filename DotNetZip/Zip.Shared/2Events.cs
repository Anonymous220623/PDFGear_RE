// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.AddProgressEventArgs
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip;

public class AddProgressEventArgs : ZipProgressEventArgs
{
  internal AddProgressEventArgs()
  {
  }

  private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor)
    : base(archiveName, flavor)
  {
  }

  internal static AddProgressEventArgs AfterEntry(
    string archiveName,
    ZipEntry entry,
    int entriesTotal)
  {
    AddProgressEventArgs progressEventArgs = new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry);
    progressEventArgs.EntriesTotal = entriesTotal;
    progressEventArgs.CurrentEntry = entry;
    return progressEventArgs;
  }

  internal static AddProgressEventArgs Started(string archiveName)
  {
    return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
  }

  internal static AddProgressEventArgs Completed(string archiveName)
  {
    return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
  }
}
