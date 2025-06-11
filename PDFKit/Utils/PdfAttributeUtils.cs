// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfAttributeUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Globalization;
using System.Linq;

#nullable disable
namespace PDFKit.Utils;

public static class PdfAttributeUtils
{
  public static string ConvertToModificationDateString(DateTimeOffset dateTime)
  {
    string str1 = dateTime.ToString("'D':yyyyMMddHHmmss");
    string str2;
    if (dateTime.Offset == TimeSpan.Zero)
    {
      str2 = "Z";
    }
    else
    {
      TimeSpan offset = dateTime.Offset;
      str2 = $"{(ValueType) (char) (offset.Ticks < 0L ? (int) '-' : (int) '+')}{offset.Hours:00}'{offset.Minutes:00}";
    }
    return str1 + str2;
  }

  public static bool TryParseModificationDate(string modificationDate, out DateTimeOffset dateTime)
  {
    dateTime = new DateTimeOffset();
    if (string.IsNullOrEmpty(modificationDate))
      return false;
    modificationDate = modificationDate.Trim();
    char ch1 = modificationDate.Last<char>();
    if (ch1 == '\'')
    {
      modificationDate = modificationDate.Substring(0, modificationDate.Length - 1);
      ch1 = modificationDate.Last<char>();
    }
    bool flag = false;
    if (ch1 == 'Z' || ch1 == 'z')
      flag = true;
    else if (modificationDate.EndsWith("00'00") && modificationDate.Length > 5)
    {
      flag = true;
      char ch2 = modificationDate[modificationDate.Length - 5];
      int length = "00'00".Length;
      if (ch2 == '+' || ch2 == '-')
        ++length;
      modificationDate = modificationDate.Substring(0, modificationDate.Length - length) + "Z";
    }
    if (flag)
      return DateTimeOffset.TryParseExact(modificationDate, "'D:'yyyyMMddHHmmssZ", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
    if (modificationDate.Length > 5)
    {
      char ch3 = modificationDate[modificationDate.Length - 6];
      TimeSpan offset = TimeSpan.Zero;
      if (ch3 == '+' || ch3 == '-')
      {
        PdfAttributeUtils.TryParseZone(modificationDate.Substring(modificationDate.Length - 6), out offset);
        modificationDate = modificationDate.Substring(0, modificationDate.Length - 6);
      }
      DateTimeOffset result;
      if (DateTimeOffset.TryParseExact(modificationDate, "'D:'yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
      {
        dateTime = new DateTimeOffset(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, offset);
        return true;
      }
    }
    return false;
  }

  private static bool TryParseZone(string zone, out TimeSpan offset)
  {
    offset = TimeSpan.Zero;
    if (string.IsNullOrEmpty(zone))
      return false;
    if (zone.Length == 1 && (zone[0] == 'Z' || zone[0] == 'z'))
      return true;
    char ch = '+';
    if (zone[0] == '+' || zone[0] == '-')
    {
      if (zone[0] == '-')
        ch = zone[0];
      zone = zone.Substring(1);
    }
    string[] strArray = zone.Split('\'');
    if (strArray.Length > 1)
    {
      string s1 = strArray[0];
      string s2 = strArray[1];
      int result1;
      int result2;
      if (int.TryParse(s1, out result1) && int.TryParse(s2, out result2))
      {
        long num = (long) result1 * 36000000000L + (long) result2 * 600000000L;
        if (ch == '-')
          num = -num;
        offset = TimeSpan.FromTicks(num);
      }
    }
    return false;
  }
}
