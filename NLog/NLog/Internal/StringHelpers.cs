// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StringHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NLog.Internal;

public static class StringHelpers
{
  [ContractAnnotation("value:null => true")]
  internal static bool IsNullOrWhiteSpace(string value) => string.IsNullOrWhiteSpace(value);

  internal static string[] SplitAndTrimTokens(this string value, char delimiter)
  {
    if (StringHelpers.IsNullOrWhiteSpace(value))
      return ArrayHelper.Empty<string>();
    if (value.IndexOf(delimiter) == -1)
      return new string[1]{ value.Trim() };
    string[] source = value.Split(new char[1]{ delimiter }, StringSplitOptions.RemoveEmptyEntries);
    for (int index = 0; index < source.Length; ++index)
    {
      source[index] = source[index].Trim();
      if (string.IsNullOrEmpty(source[index]))
        return ((IEnumerable<string>) source).Where<string>((Func<string, bool>) (s => !StringHelpers.IsNullOrWhiteSpace(s))).Select<string, string>((Func<string, string>) (s => s.Trim())).ToArray<string>();
    }
    return source;
  }
}
