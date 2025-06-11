// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.Iso8601Converter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Text;

#nullable disable
namespace XmpCore.Impl;

public static class Iso8601Converter
{
  public static IXmpDateTime Parse(string iso8601String)
  {
    return Iso8601Converter.Parse(iso8601String, (IXmpDateTime) new XmpDateTime());
  }

  public static IXmpDateTime Parse(string iso8601String, IXmpDateTime binValue)
  {
    switch (iso8601String)
    {
      case null:
        throw new XmpException("Parameter must not be null", XmpErrorCode.BadParam);
      case "":
        return binValue;
      default:
        ParseState parseState = new ParseState(iso8601String);
        if (parseState.Ch(0) == '-')
          parseState.Skip();
        int num1 = parseState.GatherInt("Invalid year in date string", 9999);
        if (parseState.HasNext && parseState.Ch() != '-')
          throw new XmpException("Invalid date string, after year", XmpErrorCode.BadValue);
        if (parseState.Ch(0) == '-')
          num1 = -num1;
        binValue.Year = num1;
        if (!parseState.HasNext)
          return binValue;
        parseState.Skip();
        int num2 = parseState.GatherInt("Invalid month in date string", 12);
        if (parseState.HasNext && parseState.Ch() != '-')
          throw new XmpException("Invalid date string, after month", XmpErrorCode.BadValue);
        binValue.Month = num2;
        if (!parseState.HasNext)
          return binValue;
        parseState.Skip();
        int num3 = parseState.GatherInt("Invalid day in date string", 31 /*0x1F*/);
        if (parseState.HasNext && parseState.Ch() != 'T')
          throw new XmpException("Invalid date string, after day", XmpErrorCode.BadValue);
        binValue.Day = num3;
        if (!parseState.HasNext)
          return binValue;
        parseState.Skip();
        int num4 = parseState.GatherInt("Invalid hour in date string", 23);
        binValue.Hour = num4;
        if (!parseState.HasNext)
          return binValue;
        if (parseState.Ch() == ':')
        {
          parseState.Skip();
          int num5 = parseState.GatherInt("Invalid minute in date string", 59);
          if (parseState.HasNext && parseState.Ch() != ':' && parseState.Ch() != 'Z' && parseState.Ch() != '+' && parseState.Ch() != '-')
            throw new XmpException("Invalid date string, after minute", XmpErrorCode.BadValue);
          binValue.Minute = num5;
        }
        if (!parseState.HasNext)
          return binValue;
        if (parseState.HasNext && parseState.Ch() == ':')
        {
          parseState.Skip();
          int num6 = parseState.GatherInt("Invalid whole seconds in date string", 59);
          if (parseState.HasNext && parseState.Ch() != '.' && parseState.Ch() != 'Z' && parseState.Ch() != '+' && parseState.Ch() != '-')
            throw new XmpException("Invalid date string, after whole seconds", XmpErrorCode.BadValue);
          binValue.Second = num6;
          if (parseState.Ch() == '.')
          {
            parseState.Skip();
            int pos = parseState.Pos;
            int num7 = parseState.GatherInt("Invalid fractional seconds in date string", 999999999);
            if (parseState.HasNext && parseState.Ch() != 'Z' && parseState.Ch() != '+' && parseState.Ch() != '-')
              throw new XmpException("Invalid date string, after fractional second", XmpErrorCode.BadValue);
            int num8;
            for (num8 = parseState.Pos - pos; num8 > 9; --num8)
              num7 /= 10;
            for (; num8 < 9; ++num8)
              num7 *= 10;
            binValue.Nanosecond = num7;
          }
        }
        else if (parseState.Ch() != 'Z' && parseState.Ch() != '+' && parseState.Ch() != '-')
          throw new XmpException("Invalid date string, after time", XmpErrorCode.BadValue);
        int num9 = 0;
        int num10 = 0;
        int num11 = 0;
        if (!parseState.HasNext)
          return binValue;
        if (parseState.Ch() == 'Z')
          parseState.Skip();
        else if (parseState.HasNext)
        {
          switch (parseState.Ch())
          {
            case '+':
              num9 = 1;
              break;
            case '-':
              num9 = -1;
              break;
            default:
              throw new XmpException("Time zone must begin with 'Z', '+', or '-'", XmpErrorCode.BadValue);
          }
          parseState.Skip();
          num10 = parseState.GatherInt("Invalid time zone hour in date string", 23);
          if (parseState.HasNext)
          {
            if (parseState.Ch() != ':')
              throw new XmpException("Invalid date string, after time zone hour", XmpErrorCode.BadValue);
            parseState.Skip();
            num11 = parseState.GatherInt("Invalid time zone minute in date string", 59);
          }
        }
        TimeSpan timeSpan = TimeSpan.FromHours((double) num10) + TimeSpan.FromMinutes((double) num11);
        if (num9 < 0)
          timeSpan = -timeSpan;
        binValue.TimeZone = TimeZoneInfo.Local;
        binValue.Offset = timeSpan;
        if (parseState.HasNext)
          throw new XmpException("Invalid date string, extra chars at end", XmpErrorCode.BadValue);
        return binValue;
    }
  }

  public static string Render(IXmpDateTime dateTime)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    if (dateTime.HasDate)
    {
      StringBuilder stringBuilder2 = stringBuilder1;
      int num1 = dateTime.Year;
      string str1 = num1.ToString("0000");
      stringBuilder2.Append(str1);
      if (dateTime.Month == 0)
        return stringBuilder1.ToString();
      stringBuilder1.Append('-');
      StringBuilder stringBuilder3 = stringBuilder1;
      num1 = dateTime.Month;
      string str2 = num1.ToString("00");
      stringBuilder3.Append(str2);
      if (dateTime.Day == 0)
        return stringBuilder1.ToString();
      stringBuilder1.Append('-');
      StringBuilder stringBuilder4 = stringBuilder1;
      num1 = dateTime.Day;
      string str3 = num1.ToString("00");
      stringBuilder4.Append(str3);
      if (dateTime.HasTime)
      {
        stringBuilder1.Append('T');
        StringBuilder stringBuilder5 = stringBuilder1;
        num1 = dateTime.Hour;
        string str4 = num1.ToString("00");
        stringBuilder5.Append(str4);
        stringBuilder1.Append(':');
        StringBuilder stringBuilder6 = stringBuilder1;
        num1 = dateTime.Minute;
        string str5 = num1.ToString("00");
        stringBuilder6.Append(str5);
        if (dateTime.Second != 0 || dateTime.Nanosecond != 0)
        {
          stringBuilder1.Append(':');
          double num2 = (double) dateTime.Second + (double) dateTime.Nanosecond / 1000000000.0;
          stringBuilder1.AppendFormat("{0:00.#########}", (object) num2);
        }
        if (dateTime.HasTimeZone)
        {
          long timeInMillis = dateTime.Calendar.GetTimeInMillis();
          int totalMilliseconds = (int) dateTime.TimeZone.GetUtcOffset(XmpDateTime.UnixTimeToDateTimeOffset(timeInMillis).DateTime).TotalMilliseconds;
          if (totalMilliseconds == 0)
          {
            stringBuilder1.Append('Z');
          }
          else
          {
            int num3 = totalMilliseconds / 3600000;
            int num4 = Math.Abs(totalMilliseconds % 3600000 / 60000);
            stringBuilder1.Append(num3.ToString("+00;-00"));
            stringBuilder1.Append(num4.ToString(":00"));
          }
        }
      }
    }
    return stringBuilder1.ToString();
  }
}
