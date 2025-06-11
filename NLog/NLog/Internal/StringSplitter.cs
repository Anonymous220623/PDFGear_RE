// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StringSplitter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal static class StringSplitter
{
  public static IEnumerable<string> SplitQuoted(
    this string text,
    char splitChar,
    char quoteChar,
    char escapeChar)
  {
    if (string.IsNullOrEmpty(text))
      return (IEnumerable<string>) ArrayHelper.Empty<string>();
    if ((int) splitChar == (int) quoteChar)
      throw new NotSupportedException("Quote character should different from split character");
    if ((int) splitChar == (int) escapeChar)
      throw new NotSupportedException("Escape character should different from split character");
    return StringSplitter.SplitQuoted2(text, splitChar, quoteChar, escapeChar);
  }

  private static IEnumerable<string> SplitQuoted2(
    string text,
    char splitChar,
    char quoteChar,
    char escapeChar)
  {
    bool inQuotedMode = false;
    bool flag1 = false;
    bool flag2 = false;
    bool doubleQuotesEscapes = (int) escapeChar == (int) quoteChar;
    StringBuilder item = new StringBuilder();
    string str = text;
    for (int index = 0; index < str.Length; ++index)
    {
      char ch = str[index];
      if ((int) ch == (int) quoteChar)
      {
        if (inQuotedMode)
        {
          if (flag1 && !doubleQuotesEscapes)
          {
            item.Append(ch);
            flag1 = false;
            flag2 = false;
          }
          else if (flag2 & doubleQuotesEscapes)
          {
            item.Append(ch);
            inQuotedMode = false;
            flag1 = false;
            flag2 = false;
          }
          else if (item.Length > 0)
          {
            inQuotedMode = false;
            yield return item.ToString();
            item.Length = 0;
            flag1 = false;
            flag2 = true;
          }
          else
          {
            inQuotedMode = false;
            flag1 = false;
            flag2 = false;
          }
        }
        else if (item.Length != 0 | flag1)
        {
          item.Append(ch);
          flag1 = false;
          flag2 = false;
        }
        else
        {
          flag1 = (int) ch == (int) escapeChar;
          flag2 = true;
          inQuotedMode = true;
        }
      }
      else if ((int) ch == (int) escapeChar)
      {
        if (flag1)
          item.Append(escapeChar);
        flag1 = true;
        flag2 = false;
      }
      else if (inQuotedMode)
      {
        item.Append(ch);
        flag1 = false;
        flag2 = false;
      }
      else if ((int) ch == (int) splitChar)
      {
        if (flag1)
          item.Append(escapeChar);
        if (item.Length > 0 || !flag2)
        {
          yield return item.ToString();
          item.Length = 0;
        }
        flag1 = false;
        flag2 = false;
      }
      else
      {
        if (flag1)
          item.Append(escapeChar);
        item.Append(ch);
        flag1 = false;
        flag2 = false;
      }
    }
    str = (string) null;
    if (flag1 && !doubleQuotesEscapes)
      item.Append(escapeChar);
    if (inQuotedMode)
    {
      if (flag2)
        item.Append(quoteChar);
      else
        item.Insert(0, quoteChar);
    }
    if (item.Length > 0 || !flag2)
      yield return item.ToString();
  }
}
