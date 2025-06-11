// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileInfoHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Internal;

internal static class FileInfoHelper
{
  internal static DateTime? LookupValidFileCreationTimeUtc<T>(
    T fileInfo,
    Func<T, DateTime?> primary,
    Func<T, DateTime?> fallback,
    Func<T, DateTime?> finalFallback = null)
  {
    DateTime? nullable = primary(fileInfo);
    if (nullable.HasValue)
    {
      DateTime dateTime = nullable.Value;
      if (dateTime.Year < 1980 && !PlatformDetector.IsWin32)
      {
        nullable = fallback(fileInfo);
        if (finalFallback != null)
        {
          if (nullable.HasValue)
          {
            dateTime = nullable.Value;
            if (dateTime.Year >= 1980)
              goto label_6;
          }
          nullable = finalFallback(fileInfo);
        }
      }
    }
label_6:
    return nullable;
  }
}
