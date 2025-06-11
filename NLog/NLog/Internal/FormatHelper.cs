// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FormatHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Collections;

#nullable disable
namespace NLog.Internal;

internal static class FormatHelper
{
  internal static string ConvertToString(object o, IFormatProvider formatProvider)
  {
    if (formatProvider == null)
    {
      if (FormatHelper.SkipFormattableToString(o))
        return o?.ToString() ?? string.Empty;
      if (o is IFormattable)
      {
        LoggingConfiguration configuration = LogManager.Configuration;
        if (configuration != null)
          formatProvider = (IFormatProvider) configuration.DefaultCultureInfo;
      }
    }
    return Convert.ToString(o, formatProvider);
  }

  private static bool SkipFormattableToString(object o)
  {
    switch (Convert.GetTypeCode(o))
    {
      case TypeCode.Empty:
        return true;
      case TypeCode.String:
        return true;
      default:
        return false;
    }
  }

  internal static string TryFormatToString(
    object value,
    string format,
    IFormatProvider formatProvider)
  {
    if (FormatHelper.SkipFormattableToString(value))
      return value?.ToString() ?? string.Empty;
    switch (value)
    {
      case IFormattable formattable:
        return formattable.ToString(format, formatProvider);
      case IEnumerable _:
        return (string) null;
      default:
        return value.ToString() ?? string.Empty;
    }
  }
}
