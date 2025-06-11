// Decompiled with JetBrains decompiler
// Type: NLog.Targets.IFileArchiveMode
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets;

internal interface IFileArchiveMode
{
  bool IsArchiveCleanupEnabled { get; }

  bool AttemptCleanupOnInitializeFile(
    string archiveFilePath,
    int maxArchiveFiles,
    int maxArchiveDays);

  string GenerateFileNameMask(string archiveFilePath);

  List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath);

  DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles);

  IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
    string archiveFilePath,
    List<DateAndSequenceArchive> existingArchiveFiles,
    int maxArchiveFiles,
    int maxArchiveDays);
}
