// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeDynamicTemplate
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal sealed class FileArchiveModeDynamicTemplate : IFileArchiveMode
{
  private readonly IFileArchiveMode _archiveHelper;

  public bool IsArchiveCleanupEnabled => this._archiveHelper.IsArchiveCleanupEnabled;

  private static string CreateDynamicTemplate(string archiveFilePath)
  {
    string extension = Path.GetExtension(archiveFilePath);
    return Path.ChangeExtension(archiveFilePath, ".{#}" + extension);
  }

  public FileArchiveModeDynamicTemplate(IFileArchiveMode archiveHelper)
  {
    this._archiveHelper = archiveHelper;
  }

  public bool AttemptCleanupOnInitializeFile(
    string archiveFilePath,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    return this._archiveHelper.AttemptCleanupOnInitializeFile(archiveFilePath, maxArchiveFiles, maxArchiveDays);
  }

  public IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
    string archiveFilePath,
    List<DateAndSequenceArchive> existingArchiveFiles,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    return this._archiveHelper.CheckArchiveCleanup(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath), existingArchiveFiles, maxArchiveFiles, maxArchiveDays);
  }

  public DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles)
  {
    return this._archiveHelper.GenerateArchiveFileName(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath), archiveDate, existingArchiveFiles);
  }

  public string GenerateFileNameMask(string archiveFilePath)
  {
    return this._archiveHelper.GenerateFileNameMask(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath));
  }

  public List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
  {
    return this._archiveHelper.GetExistingArchiveFiles(FileArchiveModeDynamicTemplate.CreateDynamicTemplate(archiveFilePath));
  }
}
