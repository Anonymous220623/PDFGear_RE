// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal abstract class FileArchiveModeBase : IFileArchiveMode
{
  private static readonly DateTime MaxAgeArchiveFileDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
  private int _lastArchiveFileCount = 65534;
  private DateTime _oldestArchiveFileDate = FileArchiveModeBase.MaxAgeArchiveFileDate;

  public bool IsArchiveCleanupEnabled { get; }

  protected FileArchiveModeBase(bool isArchiveCleanupEnabled)
  {
    this.IsArchiveCleanupEnabled = isArchiveCleanupEnabled;
  }

  public virtual bool AttemptCleanupOnInitializeFile(
    string archiveFilePath,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    if (maxArchiveFiles > 0 && this._lastArchiveFileCount++ > maxArchiveFiles)
      return true;
    if (maxArchiveDays > 0)
    {
      DateTime dateTime = TimeSource.Current.Time;
      dateTime = dateTime.Date;
      if ((dateTime.ToUniversalTime() - this._oldestArchiveFileDate.Date).TotalDays > (double) maxArchiveDays)
        return true;
    }
    return false;
  }

  public string GenerateFileNameMask(string archiveFilePath)
  {
    int archiveFileCount = this._lastArchiveFileCount;
    DateTime oldestArchiveFileDate = this._oldestArchiveFileDate;
    string fileNameMask = this.GenerateFileNameMask(archiveFilePath, this.GenerateFileNameTemplate(archiveFilePath));
    this._lastArchiveFileCount = archiveFileCount;
    this._oldestArchiveFileDate = oldestArchiveFileDate;
    return fileNameMask;
  }

  public virtual List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
  {
    this._lastArchiveFileCount = 65534;
    this._oldestArchiveFileDate = FileArchiveModeBase.MaxAgeArchiveFileDate;
    string directoryName = Path.GetDirectoryName(archiveFilePath);
    FileArchiveModeBase.FileNameTemplate fileNameTemplate = this.GenerateFileNameTemplate(archiveFilePath);
    string fileNameMask = this.GenerateFileNameMask(archiveFilePath, fileNameTemplate);
    List<DateAndSequenceArchive> existingArchiveFiles = new List<DateAndSequenceArchive>();
    if (string.IsNullOrEmpty(fileNameMask))
      return existingArchiveFiles;
    DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
    if (!directoryInfo.Exists)
      return existingArchiveFiles;
    foreach (FileInfo file in directoryInfo.GetFiles(fileNameMask))
    {
      DateAndSequenceArchive archiveFileInfo = this.GenerateArchiveFileInfo(file, fileNameTemplate);
      if (archiveFileInfo != null)
        existingArchiveFiles.Add(archiveFileInfo);
    }
    if (existingArchiveFiles.Count > 1)
      existingArchiveFiles.Sort((Comparison<DateAndSequenceArchive>) ((x, y) => FileArchiveModeBase.FileSortOrderComparison(x, y)));
    this.UpdateMaxArchiveState(existingArchiveFiles);
    return existingArchiveFiles;
  }

  protected void UpdateMaxArchiveState(List<DateAndSequenceArchive> existingArchiveFiles)
  {
    this._lastArchiveFileCount = existingArchiveFiles.Count;
    DateTime dateTime;
    if (existingArchiveFiles.Count != 0)
    {
      DateTime date = existingArchiveFiles[0].Date;
      date = date.Date;
      dateTime = date.ToUniversalTime();
    }
    else
      dateTime = DateTime.UtcNow;
    this._oldestArchiveFileDate = dateTime;
  }

  private static int FileSortOrderComparison(DateAndSequenceArchive x, DateAndSequenceArchive y)
  {
    if (x.Date != y.Date && !x.HasSameFormattedDate(y.Date))
      return x.Date.CompareTo(y.Date);
    return x.Sequence.CompareTo(y.Sequence) != 0 ? x.Sequence.CompareTo(y.Sequence) : string.CompareOrdinal(x.FileName, y.FileName);
  }

  protected virtual FileArchiveModeBase.FileNameTemplate GenerateFileNameTemplate(
    string archiveFilePath)
  {
    ++this._lastArchiveFileCount;
    return new FileArchiveModeBase.FileNameTemplate(Path.GetFileName(archiveFilePath));
  }

  protected virtual string GenerateFileNameMask(
    string archiveFilePath,
    FileArchiveModeBase.FileNameTemplate fileTemplate)
  {
    return fileTemplate != null ? fileTemplate.ReplacePattern("*") : string.Empty;
  }

  protected abstract DateAndSequenceArchive GenerateArchiveFileInfo(
    FileInfo archiveFile,
    FileArchiveModeBase.FileNameTemplate fileTemplate);

  public abstract DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles);

  public virtual IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
    string archiveFilePath,
    List<DateAndSequenceArchive> existingArchiveFiles,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    FileArchiveModeBase fileArchiveModeBase1 = this;
    if (maxArchiveFiles > 0 || maxArchiveDays > 0)
    {
      fileArchiveModeBase1.UpdateMaxArchiveState(existingArchiveFiles);
      if (existingArchiveFiles.Count != 0 && (maxArchiveFiles <= 0 || existingArchiveFiles.Count > maxArchiveFiles || maxArchiveDays > 0))
      {
        for (int i = 0; i < existingArchiveFiles.Count; ++i)
        {
          if (fileArchiveModeBase1.ShouldDeleteFile(existingArchiveFiles[i], existingArchiveFiles.Count - i, maxArchiveFiles, maxArchiveDays))
          {
            if (fileArchiveModeBase1._lastArchiveFileCount > 0)
              --fileArchiveModeBase1._lastArchiveFileCount;
            yield return existingArchiveFiles[i];
          }
          else
          {
            FileArchiveModeBase fileArchiveModeBase2 = fileArchiveModeBase1;
            DateTime date = existingArchiveFiles[i].Date;
            date = date.Date;
            DateTime universalTime = date.ToUniversalTime();
            fileArchiveModeBase2._oldestArchiveFileDate = universalTime;
            break;
          }
        }
      }
    }
  }

  private bool ShouldDeleteFile(
    DateAndSequenceArchive existingArchiveFile,
    int remainingFileCount,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    if (maxArchiveFiles > 0 && remainingFileCount > maxArchiveFiles)
      return true;
    if (maxArchiveDays > 0)
    {
      DateTime date = existingArchiveFile.Date;
      date = date.Date;
      DateTime universalTime1 = date.ToUniversalTime();
      if (universalTime1 > FileArchiveModeBase.MaxAgeArchiveFileDate)
      {
        DateTime dateTime = TimeSource.Current.Time;
        dateTime = dateTime.Date;
        DateTime universalTime2 = dateTime.ToUniversalTime();
        double totalDays = (universalTime2 - universalTime1).TotalDays;
        if (totalDays > (double) maxArchiveDays)
        {
          InternalLogger.Debug("FileTarget: Detected old file in archive. FileName={0}, FileDate={1:u}, FileDateUtc={2:u}, CurrentDateUtc={3:u}, Age={4} days", (object) existingArchiveFile.FileName, (object) existingArchiveFile.Date, (object) universalTime1, (object) universalTime2, (object) Math.Round(totalDays, 1));
          return true;
        }
      }
    }
    return false;
  }

  internal sealed class FileNameTemplate
  {
    public const string PatternStartCharacters = "{#";
    public const string PatternEndCharacters = "#}";

    public string Template { get; private set; }

    public int BeginAt { get; private set; }

    public int EndAt { get; private set; }

    private bool FoundPattern => this.BeginAt != -1 && this.EndAt != -1;

    public FileNameTemplate(string template)
    {
      this.Template = template;
      this.BeginAt = template.IndexOf("{#", StringComparison.Ordinal);
      if (this.BeginAt == -1)
        return;
      this.EndAt = template.IndexOf("#}", StringComparison.Ordinal) + "#}".Length;
    }

    public string ReplacePattern(string replacementValue)
    {
      return this.FoundPattern && !string.IsNullOrEmpty(replacementValue) ? this.Template.Substring(0, this.BeginAt) + replacementValue + this.Template.Substring(this.EndAt) : this.Template;
    }
  }
}
