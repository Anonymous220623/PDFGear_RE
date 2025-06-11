// Decompiled with JetBrains decompiler
// Type: NLog.Internal.UrlHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal static class UrlHelper
{
  private const string RFC2396ReservedMarks = ";/?:@&=+$,";
  private const string RFC3986ReservedMarks = ":/?#[]@!$&'()*+,;=";
  private const string RFC2396UnreservedMarks = "-_.!~*'()";
  private const string RFC3986UnreservedMarks = "-._~";
  private static readonly char[] hexUpperChars = new char[16 /*0x10*/]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    'A',
    'B',
    'C',
    'D',
    'E',
    'F'
  };
  private static readonly char[] hexLowerChars = new char[16 /*0x10*/]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    'a',
    'b',
    'c',
    'd',
    'e',
    'f'
  };

  public static void EscapeDataEncode(
    string source,
    StringBuilder target,
    UrlHelper.EscapeEncodingOptions options)
  {
    if (string.IsNullOrEmpty(source))
      return;
    int num = UrlHelper.Contains(options, UrlHelper.EscapeEncodingOptions.LowerCaseHex) ? 1 : 0;
    bool flag1 = UrlHelper.Contains(options, UrlHelper.EscapeEncodingOptions.SpaceAsPlus);
    bool flag2 = UrlHelper.Contains(options, UrlHelper.EscapeEncodingOptions.NLogLegacy);
    char[] charArray = (char[]) null;
    byte[] byteArray = (byte[]) null;
    char[] hexChars = num != 0 ? UrlHelper.hexLowerChars : UrlHelper.hexUpperChars;
    for (int index = 0; index < source.Length; ++index)
    {
      char ch = source[index];
      target.Append(ch);
      if (!UrlHelper.IsSimpleCharOrNumber(ch))
      {
        if (flag1 && ch == ' ')
          target[target.Length - 1] = '+';
        else if (!UrlHelper.IsAllowedChar(options, ch))
        {
          if (flag2)
          {
            UrlHelper.HandleLegacyEncoding(target, ch, hexChars);
          }
          else
          {
            if (charArray == null)
              charArray = new char[1];
            charArray[0] = ch;
            if (byteArray == null)
              byteArray = new byte[8];
            UrlHelper.WriteWideChars(target, charArray, byteArray, hexChars);
          }
        }
      }
    }
  }

  private static bool Contains(
    UrlHelper.EscapeEncodingOptions options,
    UrlHelper.EscapeEncodingOptions option)
  {
    return (options & option) == option;
  }

  private static void WriteWideChars(
    StringBuilder target,
    char[] charArray,
    byte[] byteArray,
    char[] hexChars)
  {
    int bytes = Encoding.UTF8.GetBytes(charArray, 0, 1, byteArray, 0);
    for (int index = 0; index < bytes; ++index)
    {
      byte num = byteArray[index];
      if (index == 0)
        target[target.Length - 1] = '%';
      else
        target.Append('%');
      target.Append(hexChars[((int) num & 240 /*0xF0*/) >> 4]);
      target.Append(hexChars[(int) num & 15]);
    }
  }

  private static void HandleLegacyEncoding(StringBuilder target, char ch, char[] hexChars)
  {
    if (ch < 'Ā')
    {
      target[target.Length - 1] = '%';
      target.Append(hexChars[(int) ch >> 4 & 15]);
      target.Append(hexChars[(int) ch & 15]);
    }
    else
    {
      target[target.Length - 1] = '%';
      target.Append('u');
      target.Append(hexChars[(int) ch >> 12 & 15]);
      target.Append(hexChars[(int) ch >> 8 & 15]);
      target.Append(hexChars[(int) ch >> 4 & 15]);
      target.Append(hexChars[(int) ch & 15]);
    }
  }

  private static bool IsAllowedChar(UrlHelper.EscapeEncodingOptions options, char ch)
  {
    int num = (options & UrlHelper.EscapeEncodingOptions.UriString) == UrlHelper.EscapeEncodingOptions.UriString ? 1 : 0;
    bool flag = (options & UrlHelper.EscapeEncodingOptions.LegacyRfc2396) == UrlHelper.EscapeEncodingOptions.LegacyRfc2396;
    if (num != 0)
    {
      if (!flag && "-._~".IndexOf(ch) >= 0 || flag && "-_.!~*'()".IndexOf(ch) >= 0)
        return true;
    }
    else if (!flag && ":/?#[]@!$&'()*+,;=".IndexOf(ch) >= 0 || flag && ";/?:@&=+$,".IndexOf(ch) >= 0)
      return true;
    return false;
  }

  private static bool IsSimpleCharOrNumber(char ch)
  {
    if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z')
      return true;
    return ch >= '0' && ch <= '9';
  }

  public static UrlHelper.EscapeEncodingOptions GetUriStringEncodingFlags(
    bool escapeDataNLogLegacy,
    bool spaceAsPlus,
    bool escapeDataRfc3986)
  {
    UrlHelper.EscapeEncodingOptions stringEncodingFlags = UrlHelper.EscapeEncodingOptions.UriString;
    if (escapeDataNLogLegacy)
      stringEncodingFlags |= UrlHelper.EscapeEncodingOptions.NLogLegacy;
    else if (!escapeDataRfc3986)
      stringEncodingFlags |= UrlHelper.EscapeEncodingOptions.LegacyRfc2396 | UrlHelper.EscapeEncodingOptions.LowerCaseHex;
    if (spaceAsPlus)
      stringEncodingFlags |= UrlHelper.EscapeEncodingOptions.SpaceAsPlus;
    return stringEncodingFlags;
  }

  [Flags]
  public enum EscapeEncodingOptions
  {
    None = 0,
    UriString = 1,
    LegacyRfc2396 = 2,
    LowerCaseHex = 4,
    SpaceAsPlus = 8,
    NLogLegacy = 23, // 0x00000017
  }
}
