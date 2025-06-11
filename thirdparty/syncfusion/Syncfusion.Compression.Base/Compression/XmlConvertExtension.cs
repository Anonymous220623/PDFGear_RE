// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.XmlConvertExtension
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.Compression;

internal class XmlConvertExtension
{
  internal static Regex NumberRegex = new Regex("[\\d]+", RegexOptions.IgnorePatternWhitespace);
  internal static readonly char[] WhitespaceChars = new char[4]
  {
    ' ',
    '\t',
    '\n',
    '\r'
  };

  internal static byte ToByte(string s)
  {
    byte result;
    return byte.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? result : (byte) XmlConvertExtension.GetTruncatedValue(s, (double) byte.MaxValue);
  }

  internal static short ToInt16(string s)
  {
    short result;
    return short.TryParse(s, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? result : (short) XmlConvertExtension.GetTruncatedValue(s, (double) short.MaxValue);
  }

  internal static int ToInt32(string s)
  {
    int result;
    return int.TryParse(s, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? result : (int) XmlConvertExtension.GetTruncatedValue(s, (double) int.MaxValue);
  }

  internal static long ToInt64(string s)
  {
    long result;
    return long.TryParse(s, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? result : (long) XmlConvertExtension.GetTruncatedValue(s, (double) long.MaxValue);
  }

  [CLSCompliant(false)]
  internal static ushort ToUInt16(string s)
  {
    ushort result;
    return ushort.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? result : (ushort) XmlConvertExtension.GetTruncatedValue(s, (double) ushort.MaxValue);
  }

  [CLSCompliant(false)]
  internal static uint ToUInt32(string s)
  {
    uint result;
    return uint.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? result : (uint) XmlConvertExtension.GetTruncatedValue(s, (double) uint.MaxValue);
  }

  internal static string TrimString(string value)
  {
    return value.Trim(XmlConvertExtension.WhitespaceChars);
  }

  internal static float ToSingle(string s)
  {
    s = XmlConvertExtension.TrimString(s);
    switch (s)
    {
      case "-INF":
        return float.NegativeInfinity;
      case "INF":
        return float.PositiveInfinity;
      default:
        float result;
        if (!float.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
          result = (float) XmlConvertExtension.GetTruncatedValue(s, 3.4028234663852886E+38);
        return (double) result == 0.0 && s[0] == '-' ? -0.0f : result;
    }
  }

  internal static double ToDouble(string s)
  {
    s = XmlConvertExtension.TrimString(s);
    switch (s)
    {
      case "-INF":
        return double.NegativeInfinity;
      case "INF":
        return double.PositiveInfinity;
      default:
        double result;
        if (!double.TryParse(s, NumberStyles.Float, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
          result = XmlConvertExtension.GetTruncatedValue(s, double.MaxValue);
        return result == 0.0 && s[0] == '-' ? -0.0 : result;
    }
  }

  internal static bool ToBoolean(string s)
  {
    s = XmlConvertExtension.TrimString(s);
    switch (s)
    {
      case "1":
      case "true":
        return true;
      case "0":
      case "false":
        return false;
      default:
        return true;
    }
  }

  internal static double GetTruncatedValue(string input, double maxValue)
  {
    Match match = XmlConvertExtension.NumberRegex.Match(input);
    double result = 0.0;
    if (match.Success && double.TryParse(match.Value, out result) && result > maxValue)
      result %= maxValue;
    return result;
  }

  internal static DateTimeOffset ToDateTimeOffset(string value)
  {
    return string.IsNullOrEmpty(value) || XmlConvertExtension.IsWhiteSpace(value) ? new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero) : XmlConvert.ToDateTimeOffset(value);
  }

  private static bool IsWhiteSpace(string value)
  {
    for (int index = 0; index < value.Length; ++index)
    {
      if (!char.IsWhiteSpace(value[index]))
        return false;
    }
    return true;
  }

  internal static DateTime ToDateTime(string value, XmlDateTimeSerializationMode dateTimeOption)
  {
    return string.IsNullOrEmpty(value) || XmlConvertExtension.IsWhiteSpace(value) ? new DateTime(1900, 1, 1, 0, 0, 0) : XmlConvert.ToDateTime(value, dateTimeOption);
  }
}
