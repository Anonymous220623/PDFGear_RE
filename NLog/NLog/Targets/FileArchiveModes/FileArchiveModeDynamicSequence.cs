// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeDynamicSequence
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal sealed class FileArchiveModeDynamicSequence : FileArchiveModeBase
{
  private readonly ArchiveNumberingMode _archiveNumbering;
  private readonly string _archiveDateFormat;
  private readonly bool _customArchiveFileName;

  public FileArchiveModeDynamicSequence(
    ArchiveNumberingMode archiveNumbering,
    string archiveDateFormat,
    bool customArchiveFileName,
    bool archiveCleanupEnabled)
    : base(archiveCleanupEnabled)
  {
    this._archiveNumbering = archiveNumbering;
    this._archiveDateFormat = archiveDateFormat;
    this._customArchiveFileName = customArchiveFileName;
  }

  private static bool RemoveNonLetters(
    string fileName,
    int startPosition,
    StringBuilder sb,
    out int digitsRemoved)
  {
    digitsRemoved = 0;
    sb.ClearBuilder();
    for (int index = 0; index < startPosition; ++index)
      sb.Append(fileName[index]);
    bool? nullable = new bool?();
    for (int index = startPosition; index < fileName.Length; ++index)
    {
      char c = fileName[index];
      if (char.IsDigit(c))
      {
        if (!nullable.HasValue)
        {
          nullable = new bool?(true);
          digitsRemoved = 1;
          sb.Append('{');
          sb.Append('#');
          sb.Append('}');
        }
        else if (!nullable.Value)
          sb.Append(c);
        else
          ++digitsRemoved;
      }
      else if (!char.IsLetter(c))
      {
        if (!nullable.HasValue || !nullable.Value)
          sb.Append(c);
      }
      else
      {
        if (nullable.HasValue)
          nullable = new bool?(false);
        sb.Append(c);
      }
    }
    return nullable.HasValue;
  }

  protected override FileArchiveModeBase.FileNameTemplate GenerateFileNameTemplate(
    string archiveFilePath)
  {
    string withoutExtension = Path.GetFileNameWithoutExtension(archiveFilePath);
    StringBuilder stringBuilder = new StringBuilder();
    int startPosition1 = 0;
    int num = int.MaxValue;
    int digitsRemoved;
    for (int startPosition2 = 0; startPosition2 < withoutExtension.Length && (FileArchiveModeDynamicSequence.RemoveNonLetters(withoutExtension, startPosition2, stringBuilder, out digitsRemoved) || startPosition2 != 0); ++startPosition2)
    {
      if (digitsRemoved > 1 && stringBuilder.Length < num)
      {
        startPosition1 = startPosition2;
        num = stringBuilder.Length;
      }
    }
    FileArchiveModeDynamicSequence.RemoveNonLetters(withoutExtension, startPosition1, stringBuilder, out digitsRemoved);
    if (digitsRemoved <= 1)
    {
      stringBuilder.ClearBuilder();
      stringBuilder.Append(withoutExtension);
    }
    switch (this._archiveNumbering)
    {
      case ArchiveNumberingMode.Sequence:
      case ArchiveNumberingMode.Rolling:
      case ArchiveNumberingMode.DateAndSequence:
        if (stringBuilder.Length < 3 || stringBuilder[stringBuilder.Length - 3] != '{' && stringBuilder[stringBuilder.Length - 2] != '#' && stringBuilder[stringBuilder.Length - 1] != '}')
        {
          if (digitsRemoved <= 1)
          {
            stringBuilder.Append("{");
            stringBuilder.Append("#");
            stringBuilder.Append("}");
            break;
          }
          stringBuilder.Append('*');
          break;
        }
        break;
    }
    stringBuilder.Append(Path.GetExtension(archiveFilePath));
    return base.GenerateFileNameTemplate(stringBuilder.ToString());
  }

  protected override DateAndSequenceArchive GenerateArchiveFileInfo(
    FileInfo archiveFile,
    FileArchiveModeBase.FileNameTemplate fileTemplate)
  {
    if (fileTemplate != null && fileTemplate.EndAt > 0)
    {
      string name = archiveFile.Name;
      int index1 = 0;
      for (int index2 = 0; index2 < name.Length; ++index2)
      {
        char c1 = name[index2];
        if (index1 >= fileTemplate.Template.Length)
        {
          if (char.IsLetter(c1))
            return (DateAndSequenceArchive) null;
          break;
        }
        char c2;
        if (index1 < fileTemplate.EndAt && index2 >= fileTemplate.BeginAt)
        {
          if (char.IsLetter(c1))
          {
            index1 = fileTemplate.EndAt;
            while (index1 < fileTemplate.Template.Length)
            {
              c2 = fileTemplate.Template[index1];
              ++index1;
              if (char.IsLetter(c2))
                goto label_12;
            }
            return (DateAndSequenceArchive) null;
          }
          continue;
        }
        c2 = fileTemplate.Template[index1];
        ++index1;
label_12:
        if ((int) c1 != (int) c2 && (int) char.ToLowerInvariant(c1) != (int) char.ToLowerInvariant(c2))
        {
          if (c2 != '*' || char.IsLetter(c1))
            return (DateAndSequenceArchive) null;
          break;
        }
      }
    }
    int numberFromFileName = FileArchiveModeDynamicSequence.ExtractArchiveNumberFromFileName(archiveFile.FullName);
    InternalLogger.Trace<int, string>("FileTarget: extracted sequenceNumber: {0} from file '{1}'", numberFromFileName, archiveFile.FullName);
    DateTime date = archiveFile.LookupValidFileCreationTimeUtc();
    return new DateAndSequenceArchive(archiveFile.FullName, date, string.Empty, numberFromFileName > 0 ? numberFromFileName : 0);
  }

  private static int ExtractArchiveNumberFromFileName(string archiveFileName)
  {
    archiveFileName = Path.GetFileName(archiveFileName);
    int num1 = archiveFileName.LastIndexOf('.');
    if (num1 == -1)
      return 0;
    int num2 = archiveFileName.LastIndexOf('.', num1 - 1);
    int result;
    return !int.TryParse(num2 == -1 ? archiveFileName.Substring(num1 + 1) : archiveFileName.Substring(num2 + 1, num1 - num2 - 1), out result) ? 0 : result;
  }

  public override DateAndSequenceArchive GenerateArchiveFileName(
    string archiveFilePath,
    DateTime archiveDate,
    List<DateAndSequenceArchive> existingArchiveFiles)
  {
    int num = this.GetStartSequenceNo() - 1;
    string fileName1 = Path.GetFileName(archiveFilePath);
    foreach (DateAndSequenceArchive existingArchiveFile in existingArchiveFiles)
    {
      string fileName2 = Path.GetFileName(existingArchiveFile.FileName);
      if (string.Equals(fileName2, fileName1, StringComparison.OrdinalIgnoreCase))
      {
        num = Math.Max(num, existingArchiveFile.Sequence + this.GetStartSequenceNo());
      }
      else
      {
        string extension = Path.GetExtension(fileName2);
        if (string.Equals(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName2)) + extension, fileName1, StringComparison.OrdinalIgnoreCase))
          num = Math.Max(num, existingArchiveFile.Sequence + 1);
      }
    }
    archiveFilePath = Path.GetFullPath(archiveFilePath);
    if (num >= this.GetStartSequenceNo())
      archiveFilePath = Path.Combine(Path.GetDirectoryName(archiveFilePath), $"{Path.GetFileNameWithoutExtension(fileName1)}.{num.ToString((IFormatProvider) CultureInfo.InvariantCulture)}{Path.GetExtension(fileName1)}");
    return new DateAndSequenceArchive(archiveFilePath, archiveDate, this._archiveDateFormat, num);
  }

  private int GetStartSequenceNo() => !this._customArchiveFileName ? 0 : 1;
}
