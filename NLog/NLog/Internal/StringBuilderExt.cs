// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StringBuilderExt
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.MessageTemplates;
using System;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal static class StringBuilderExt
{
  private static readonly char[] charToInt = new char[10]
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
    '9'
  };

  public static void AppendFormattedValue(
    this StringBuilder builder,
    object value,
    string format,
    IFormatProvider formatProvider)
  {
    if (value is string && string.IsNullOrEmpty(format))
      builder.Append(value);
    else if (format == "@")
    {
      ValueFormatter.Instance.FormatValue(value, (string) null, CaptureType.Serialize, formatProvider, builder);
    }
    else
    {
      if (value == null)
        return;
      ValueFormatter.Instance.FormatValue(value, format, CaptureType.Normal, formatProvider, builder);
    }
  }

  public static void AppendInvariant(this StringBuilder builder, int value)
  {
    if (value < 0)
    {
      builder.Append('-');
      uint num = (uint) (-1 - value + 1);
      builder.AppendInvariant(num);
    }
    else
      builder.AppendInvariant((uint) value);
  }

  public static void AppendInvariant(this StringBuilder builder, uint value)
  {
    if (value == 0U)
    {
      builder.Append('0');
    }
    else
    {
      int digitCount = StringBuilderExt.CalculateDigitCount(value);
      StringBuilderExt.ApppendValueWithDigitCount(builder, value, digitCount);
    }
  }

  private static int CalculateDigitCount(uint value)
  {
    int digitCount = 0;
    uint num = value;
    do
    {
      num /= 10U;
      ++digitCount;
    }
    while (num > 0U);
    return digitCount;
  }

  private static void ApppendValueWithDigitCount(StringBuilder builder, uint value, int digitCount)
  {
    builder.Append('0', digitCount);
    int length = builder.Length;
    for (; digitCount > 0; --digitCount)
    {
      --length;
      builder[length] = StringBuilderExt.charToInt[(int) (value % 10U)];
      value /= 10U;
    }
  }

  public static void AppendXmlDateTimeRoundTrip(this StringBuilder builder, DateTime dateTime)
  {
    dateTime = dateTime.Kind != DateTimeKind.Unspecified ? dateTime.ToUniversalTime() : new DateTime(dateTime.Ticks, DateTimeKind.Utc);
    builder.Append4DigitsZeroPadded(dateTime.Year);
    builder.Append('-');
    builder.Append2DigitsZeroPadded(dateTime.Month);
    builder.Append('-');
    builder.Append2DigitsZeroPadded(dateTime.Day);
    builder.Append('T');
    builder.Append2DigitsZeroPadded(dateTime.Hour);
    builder.Append(':');
    builder.Append2DigitsZeroPadded(dateTime.Minute);
    builder.Append(':');
    builder.Append2DigitsZeroPadded(dateTime.Second);
    int num1 = (int) (dateTime.Ticks % 10000000L);
    if (num1 > 0)
    {
      builder.Append('.');
      int num2 = 7;
      for (; num1 % 10 == 0; num1 /= 10)
        --num2;
      int digitCount = StringBuilderExt.CalculateDigitCount((uint) num1);
      if (num2 > digitCount)
        builder.Append('0', num2 - digitCount);
      StringBuilderExt.ApppendValueWithDigitCount(builder, (uint) num1, digitCount);
    }
    builder.Append('Z');
  }

  public static void ClearBuilder(this StringBuilder builder)
  {
    try
    {
      builder.Clear();
    }
    catch
    {
      if (builder.Length > 1)
        builder.Remove(0, builder.Length - 1);
      builder.Remove(0, builder.Length);
    }
  }

  public static void CopyToStream(
    this StringBuilder builder,
    MemoryStream ms,
    Encoding encoding,
    char[] transformBuffer)
  {
    if (transformBuffer != null)
    {
      int maxByteCount = encoding.GetMaxByteCount(builder.Length);
      ms.SetLength(ms.Position + (long) maxByteCount);
      for (int sourceIndex = 0; sourceIndex < builder.Length; sourceIndex += transformBuffer.Length)
      {
        int num = Math.Min(builder.Length - sourceIndex, transformBuffer.Length);
        builder.CopyTo(sourceIndex, transformBuffer, 0, num);
        int bytes = encoding.GetBytes(transformBuffer, 0, num, ms.GetBuffer(), (int) ms.Position);
        ms.Position += (long) bytes;
      }
      if (ms.Position == ms.Length)
        return;
      ms.SetLength(ms.Position);
    }
    else
    {
      string s = builder.ToString();
      byte[] bytes = encoding.GetBytes(s);
      ms.Write(bytes, 0, bytes.Length);
    }
  }

  public static void CopyToBuffer(
    this StringBuilder builder,
    char[] destination,
    int destinationIndex)
  {
    builder.CopyTo(0, destination, destinationIndex, builder.Length);
  }

  public static void CopyTo(this StringBuilder builder, StringBuilder destination)
  {
    int length = builder.Length;
    if (length <= 0)
      return;
    destination.EnsureCapacity(length + destination.Length);
    if (length < 8)
    {
      for (int index = 0; index < length; ++index)
        destination.Append(builder[index]);
    }
    else if (length < 512 /*0x0200*/)
    {
      destination.Append(builder.ToString());
    }
    else
    {
      char[] destination1 = new char[256 /*0x0100*/];
      for (int sourceIndex = 0; sourceIndex < length; sourceIndex += destination1.Length)
      {
        int num = Math.Min(length - sourceIndex, destination1.Length);
        builder.CopyTo(sourceIndex, destination1, 0, num);
        destination.Append(destination1, 0, num);
      }
    }
  }

  public static int IndexOf(this StringBuilder builder, char needle, int startPos = 0)
  {
    for (int index = startPos; index < builder.Length; ++index)
    {
      if ((int) builder[index] == (int) needle)
        return index;
    }
    return -1;
  }

  public static int IndexOfAny(this StringBuilder builder, char[] needles, int startPos = 0)
  {
    for (int index = startPos; index < builder.Length; ++index)
    {
      if (StringBuilderExt.CharArrayContains(builder[index], needles))
        return index;
    }
    return -1;
  }

  private static bool CharArrayContains(char searchChar, char[] needles)
  {
    for (int index = 0; index < needles.Length; ++index)
    {
      if ((int) needles[index] == (int) searchChar)
        return true;
    }
    return false;
  }

  public static bool EqualTo(this StringBuilder builder, StringBuilder other)
  {
    if (builder.Length != other.Length)
      return false;
    for (int index = 0; index < builder.Length; ++index)
    {
      if ((int) builder[index] != (int) other[index])
        return false;
    }
    return true;
  }

  public static bool EqualTo(this StringBuilder builder, string other)
  {
    if (builder.Length != other.Length)
      return false;
    for (int index = 0; index < other.Length; ++index)
    {
      if ((int) builder[index] != (int) other[index])
        return false;
    }
    return true;
  }

  internal static void Append2DigitsZeroPadded(this StringBuilder builder, int number)
  {
    builder.Append((char) (number / 10 + 48 /*0x30*/));
    builder.Append((char) (number % 10 + 48 /*0x30*/));
  }

  internal static void Append4DigitsZeroPadded(this StringBuilder builder, int number)
  {
    builder.Append((char) (number / 1000 % 10 + 48 /*0x30*/));
    builder.Append((char) (number / 100 % 10 + 48 /*0x30*/));
    builder.Append((char) (number / 10 % 10 + 48 /*0x30*/));
    builder.Append((char) (number / 1 % 10 + 48 /*0x30*/));
  }

  internal static void AppendIntegerAsString(
    this StringBuilder sb,
    IConvertible value,
    TypeCode objTypeCode)
  {
    switch (objTypeCode)
    {
      case TypeCode.SByte:
        sb.AppendInvariant((int) value.ToSByte((IFormatProvider) CultureInfo.InvariantCulture));
        break;
      case TypeCode.Byte:
        sb.AppendInvariant((int) value.ToByte((IFormatProvider) CultureInfo.InvariantCulture));
        break;
      case TypeCode.Int16:
        sb.AppendInvariant((int) value.ToInt16((IFormatProvider) CultureInfo.InvariantCulture));
        break;
      case TypeCode.UInt16:
        sb.AppendInvariant((int) value.ToUInt16((IFormatProvider) CultureInfo.InvariantCulture));
        break;
      case TypeCode.Int32:
        sb.AppendInvariant(value.ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
        break;
      case TypeCode.UInt32:
        sb.AppendInvariant(value.ToUInt32((IFormatProvider) CultureInfo.InvariantCulture));
        break;
      case TypeCode.Int64:
        long int64 = value.ToInt64((IFormatProvider) CultureInfo.InvariantCulture);
        if (int64 < (long) int.MaxValue && int64 > (long) int.MinValue)
        {
          sb.AppendInvariant((int) int64);
          break;
        }
        sb.Append(int64);
        break;
      case TypeCode.UInt64:
        ulong uint64 = value.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture);
        if (uint64 < (ulong) uint.MaxValue)
        {
          sb.AppendInvariant((uint) uint64);
          break;
        }
        sb.Append(uint64);
        break;
      default:
        sb.Append(XmlHelper.XmlConvertToString(value, objTypeCode));
        break;
    }
  }

  public static void TrimRight(this StringBuilder sb, int startPos = 0)
  {
    int index = sb.Length - 1;
    while (index >= startPos && char.IsWhiteSpace(sb[index]))
      --index;
    if (index >= sb.Length - 1)
      return;
    sb.Length = index + 1;
  }
}
