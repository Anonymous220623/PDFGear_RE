// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventType
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip;

public enum ZipProgressEventType
{
  Adding_Started,
  Adding_AfterAddEntry,
  Adding_Completed,
  Reading_Started,
  Reading_BeforeReadEntry,
  Reading_AfterReadEntry,
  Reading_Completed,
  Reading_ArchiveBytesRead,
  Saving_Started,
  Saving_BeforeWriteEntry,
  Saving_AfterWriteEntry,
  Saving_Completed,
  Saving_AfterSaveTempArchive,
  Saving_BeforeRenameTempArchive,
  Saving_AfterRenameTempArchive,
  Saving_AfterCompileSelfExtractor,
  Saving_EntryBytesRead,
  Extracting_BeforeExtractEntry,
  Extracting_AfterExtractEntry,
  Extracting_ExtractEntryWouldOverwrite,
  Extracting_EntryBytesWritten,
  Extracting_BeforeExtractAll,
  Extracting_AfterExtractAll,
  Error_Saving,
}
