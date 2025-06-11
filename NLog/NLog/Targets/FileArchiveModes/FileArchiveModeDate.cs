// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeDate
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Time;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal sealed class FileArchiveModeDate : FileArchiveModeBase
{
  private readonly string _archiveDateFormat;

  public FileArchiveModeDate(string archiveDateFormat, bool isArchiveCleanupEnabled)
    : base(isArchiveCleanupEnabled)
  {
    this._archiveDateFormat = archiveDateFormat;
  }

  public override List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
  {
    return this.IsArchiveCleanupEnabled ? base.GetExistingArchiveFiles(archiveFilePath) : new List<DateAndSequenceArchive>();
  }

  protected override DateAndSequenceArchive GenerateArchiveFileInfo(
    FileInfo archiveFile,
    FileArchiveModeBase.FileNameTemplate fileTemplate)
  {
    string str = Path.GetFileName(archiveFile.FullName) ?? "";
    int startIndex = fileTemplate.ReplacePattern("*").LastIndexOf('*');
    DateTime result;
    if (startIndex + this._archiveDateFormat.Length > str.Length || !DateTime.TryParseExact(str.Substring(startIndex, this._archiveDateFormat.Length), this._archiveDateFormat, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
      return (DateAndSequenceArchive) null;
    result = DateTime.SpecifyKind(result, TimeSource.Current.Time.Kind);
    return new DateAndSequenceArchive(archiveFile.FullName, result, this._archiveDateFormat, -1);
  }

  public override DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles)
  {
    FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
    archiveFilePath = Path.Combine(Path.GetDirectoryName(archiveFilePath), fileNameTemplate.ReplacePattern("*").Replace("*", archiveDate.ToString(this._archiveDateFormat)));
    archiveFilePath = Path.GetFullPath(archiveFilePath);
    return new DateAndSequenceArchive(archiveFilePath, archiveDate, this._archiveDateFormat, 0);
  }
}
