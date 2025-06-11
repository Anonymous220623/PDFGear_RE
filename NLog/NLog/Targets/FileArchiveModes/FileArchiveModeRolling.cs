// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeRolling
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal sealed class FileArchiveModeRolling : IFileArchiveMode
{
  public bool IsArchiveCleanupEnabled => true;

  public bool AttemptCleanupOnInitializeFile(
    string archiveFilePath,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    return false;
  }

  public string GenerateFileNameMask(string archiveFilePath)
  {
    return new FileArchiveModeBase.FileNameTemplate(Path.GetFileName(archiveFilePath)).ReplacePattern("*");
  }

  public List<DateAndSequenceArchive> GetExistingArchiveFiles(string archiveFilePath)
  {
    List<DateAndSequenceArchive> existingArchiveFiles = new List<DateAndSequenceArchive>();
    for (int sequence = 0; sequence < int.MaxValue; ++sequence)
    {
      FileInfo fileInfo = new FileInfo(FileArchiveModeRolling.ReplaceNumberPattern(archiveFilePath, sequence));
      if (fileInfo.Exists)
        existingArchiveFiles.Add(new DateAndSequenceArchive(fileInfo.FullName, DateTime.MinValue, string.Empty, sequence));
      else
        break;
    }
    return existingArchiveFiles;
  }

  private static string ReplaceNumberPattern(string pattern, int value)
  {
    int length = pattern.IndexOf("{#", StringComparison.Ordinal);
    int startIndex = pattern.IndexOf("#}", StringComparison.Ordinal) + 2;
    int totalWidth = startIndex - length - 2;
    return pattern.Substring(0, length) + Convert.ToString(value, 10).PadLeft(totalWidth, '0') + pattern.Substring(startIndex);
  }

  public DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles)
  {
    if (existingArchiveFiles.Count > 0)
    {
      int num = existingArchiveFiles[existingArchiveFiles.Count - 1].Sequence + 1;
      string fileName = FileArchiveModeRolling.ReplaceNumberPattern(archiveFilePath, num);
      existingArchiveFiles.Add(new DateAndSequenceArchive(fileName, DateTime.MinValue, string.Empty, int.MaxValue));
    }
    return new DateAndSequenceArchive(Path.GetFullPath(FileArchiveModeRolling.ReplaceNumberPattern(archiveFilePath, 0)), DateTime.MinValue, string.Empty, int.MinValue);
  }

  public IEnumerable<DateAndSequenceArchive> CheckArchiveCleanup(
    string archiveFilePath,
    List<DateAndSequenceArchive> existingArchiveFiles,
    int maxArchiveFiles,
    int maxArchiveDays)
  {
    if (existingArchiveFiles.Count > 1)
    {
      existingArchiveFiles.Sort((Comparison<DateAndSequenceArchive>) ((x, y) => x.Sequence.CompareTo(y.Sequence)));
      if (maxArchiveFiles > 0 && existingArchiveFiles.Count > maxArchiveFiles)
      {
        for (int i = 0; i < existingArchiveFiles.Count; ++i)
        {
          if (existingArchiveFiles[i].Sequence != int.MinValue && existingArchiveFiles[i].Sequence != int.MaxValue && i >= maxArchiveFiles)
            yield return existingArchiveFiles[i];
        }
      }
      if (existingArchiveFiles.Count > 1 && existingArchiveFiles[0].Sequence == int.MinValue)
      {
        string destFileName = string.Empty;
        int num = existingArchiveFiles.Count - 1;
        if (maxArchiveFiles > 0 && num > maxArchiveFiles)
          num = maxArchiveFiles;
        for (int index = num; index >= 1; --index)
        {
          string fileName = existingArchiveFiles[index].FileName;
          if (!string.IsNullOrEmpty(destFileName))
          {
            InternalLogger.Info<string, string>("Roll archive {0} to {1}", fileName, destFileName);
            File.Move(fileName, destFileName);
          }
          destFileName = fileName;
        }
      }
    }
  }
}
