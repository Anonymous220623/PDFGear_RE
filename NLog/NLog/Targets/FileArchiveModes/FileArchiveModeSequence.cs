// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeSequence
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal sealed class FileArchiveModeSequence : FileArchiveModeBase
{
  private readonly string _archiveDateFormat;

  public FileArchiveModeSequence(string archiveDateFormat, bool isArchiveCleanupEnabled)
    : base(isArchiveCleanupEnabled)
  {
    this._archiveDateFormat = archiveDateFormat;
  }

  public override bool AttemptCleanupOnInitializeFile(
    string archiveFilePath,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    return false;
  }

  protected override DateAndSequenceArchive GenerateArchiveFileInfo(
    FileInfo archiveFile,
    FileArchiveModeBase.FileNameTemplate fileTemplate)
  {
    string str1 = Path.GetFileName(archiveFile.FullName) ?? "";
    int num = fileTemplate.Template.Length - fileTemplate.EndAt;
    string str2 = str1.Substring(fileTemplate.BeginAt, str1.Length - num - fileTemplate.BeginAt);
    int int32;
    try
    {
      int32 = Convert.ToInt32(str2, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    catch (FormatException ex)
    {
      return (DateAndSequenceArchive) null;
    }
    return new DateAndSequenceArchive(archiveFile.FullName, DateTime.MinValue, string.Empty, int32);
  }

  public override DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles)
  {
    int num = 0;
    FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
    foreach (DateAndSequenceArchive existingArchiveFile in existingArchiveFiles)
      num = Math.Max(num, existingArchiveFile.Sequence + 1);
    int totalWidth = fileNameTemplate.EndAt - fileNameTemplate.BeginAt - 2;
    string newValue = num.ToString().PadLeft(totalWidth, '0');
    archiveFilePath = Path.Combine(Path.GetDirectoryName(archiveFilePath), fileNameTemplate.ReplacePattern("*").Replace("*", newValue));
    archiveFilePath = Path.GetFullPath(archiveFilePath);
    return new DateAndSequenceArchive(archiveFilePath, archiveDate, this._archiveDateFormat, num);
  }
}
