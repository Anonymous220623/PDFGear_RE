// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileArchiveModes.FileArchiveModeFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;

#nullable disable
namespace NLog.Targets.FileArchiveModes;

internal static class FileArchiveModeFactory
{
  public static IFileArchiveMode CreateArchiveStyle(
    string archiveFilePath,
    ArchiveNumberingMode archiveNumbering,
    string dateFormat,
    bool customArchiveFileName,
    bool archiveCleanupEnabled)
  {
    if (FileArchiveModeFactory.ContainsFileNamePattern(archiveFilePath))
    {
      IFileArchiveMode strictFileArchiveMode = FileArchiveModeFactory.CreateStrictFileArchiveMode(archiveNumbering, dateFormat, archiveCleanupEnabled);
      if (strictFileArchiveMode != null)
        return strictFileArchiveMode;
    }
    if (archiveNumbering != ArchiveNumberingMode.Sequence)
    {
      if (!customArchiveFileName)
      {
        IFileArchiveMode strictFileArchiveMode = FileArchiveModeFactory.CreateStrictFileArchiveMode(archiveNumbering, dateFormat, archiveCleanupEnabled);
        if (strictFileArchiveMode != null)
          return (IFileArchiveMode) new FileArchiveModeDynamicTemplate(strictFileArchiveMode);
      }
      else
        InternalLogger.Info<string>("FileTarget: Pattern {{#}} is missing in ArchiveFileName `{0}` (Fallback to dynamic wildcard)", archiveFilePath);
    }
    return (IFileArchiveMode) new FileArchiveModeDynamicSequence(archiveNumbering, dateFormat, customArchiveFileName, archiveCleanupEnabled);
  }

  private static IFileArchiveMode CreateStrictFileArchiveMode(
    ArchiveNumberingMode archiveNumbering,
    string dateFormat,
    bool archiveCleanupEnabled)
  {
    switch (archiveNumbering)
    {
      case ArchiveNumberingMode.Sequence:
        return (IFileArchiveMode) new FileArchiveModeSequence(dateFormat, archiveCleanupEnabled);
      case ArchiveNumberingMode.Rolling:
        return (IFileArchiveMode) new FileArchiveModeRolling();
      case ArchiveNumberingMode.Date:
        return (IFileArchiveMode) new FileArchiveModeDate(dateFormat, archiveCleanupEnabled);
      case ArchiveNumberingMode.DateAndSequence:
        return (IFileArchiveMode) new FileArchiveModeDateAndSequence(dateFormat, archiveCleanupEnabled);
      default:
        return (IFileArchiveMode) null;
    }
  }

  public static bool ContainsFileNamePattern(string fileName)
  {
    int num1 = fileName.IndexOf("{#", StringComparison.Ordinal);
    int num2 = fileName.IndexOf("#}", StringComparison.Ordinal);
    return num1 != -1 && num2 != -1 && num1 < num2;
  }
}
