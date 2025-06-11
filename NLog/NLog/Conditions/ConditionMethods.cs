// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionMethods
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.Conditions;

[ConditionMethods]
public static class ConditionMethods
{
  [ConditionMethod("equals")]
  public static bool Equals2(object firstValue, object secondValue)
  {
    return firstValue.Equals(secondValue);
  }

  [ConditionMethod("strequals")]
  public static bool Equals2(string firstValue, string secondValue, bool ignoreCase = false)
  {
    bool flag = ignoreCase;
    return firstValue.Equals(secondValue, flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
  }

  [ConditionMethod("contains")]
  public static bool Contains(string haystack, string needle, bool ignoreCase = true)
  {
    bool flag = ignoreCase;
    return haystack.IndexOf(needle, flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0;
  }

  [ConditionMethod("starts-with")]
  public static bool StartsWith(string haystack, string needle, bool ignoreCase = true)
  {
    bool flag = ignoreCase;
    return haystack.StartsWith(needle, flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
  }

  [ConditionMethod("ends-with")]
  public static bool EndsWith(string haystack, string needle, bool ignoreCase = true)
  {
    bool flag = ignoreCase;
    return haystack.EndsWith(needle, flag ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
  }

  [ConditionMethod("length")]
  public static int Length(string text) => text.Length;

  [ConditionMethod("regex-matches")]
  public static bool RegexMatches(string input, string pattern, string options = "")
  {
    RegexOptions regexOptions = ConditionMethods.ParseRegexOptions(options);
    return Regex.IsMatch(input, pattern, regexOptions);
  }

  private static RegexOptions ParseRegexOptions(string options)
  {
    return string.IsNullOrEmpty(options) ? RegexOptions.None : (RegexOptions) Enum.Parse(typeof (RegexOptions), options, true);
  }
}
