// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileInfoExt
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal;

internal static class FileInfoExt
{
  public static DateTime GetLastWriteTimeUtc(this FileInfo fileInfo) => fileInfo.LastWriteTimeUtc;

  public static DateTime GetCreationTimeUtc(this FileInfo fileInfo) => fileInfo.CreationTimeUtc;

  public static DateTime LookupValidFileCreationTimeUtc(this FileInfo fileInfo)
  {
    return FileInfoHelper.LookupValidFileCreationTimeUtc<FileInfo>(fileInfo, (Func<FileInfo, DateTime?>) (f => new DateTime?(f.GetCreationTimeUtc())), (Func<FileInfo, DateTime?>) (f => new DateTime?(f.GetLastWriteTimeUtc()))).Value;
  }

  public static DateTime LookupValidFileCreationTimeUtc(
    this FileInfo fileInfo,
    DateTime? fallbackTime)
  {
    DateTime? nullable = fallbackTime;
    DateTime minValue = DateTime.MinValue;
    return (nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0 ? FileInfoHelper.LookupValidFileCreationTimeUtc<FileInfo>(fileInfo, (Func<FileInfo, DateTime?>) (f => new DateTime?(f.GetCreationTimeUtc())), (Func<FileInfo, DateTime?>) (f => new DateTime?(fallbackTime.Value)), (Func<FileInfo, DateTime?>) (f => new DateTime?(f.GetLastWriteTimeUtc()))).Value : fileInfo.LookupValidFileCreationTimeUtc();
  }
}
