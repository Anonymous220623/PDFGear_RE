// Decompiled with JetBrains decompiler
// Type: NLog.Internal.XmlHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Globalization;
using System.Text;
using System.Xml;

#nullable disable
namespace NLog.Internal;

public static class XmlHelper
{
  private static readonly char[] XmlEscapeChars = new char[5]
  {
    '<',
    '>',
    '&',
    '\'',
    '"'
  };
  private static readonly char[] XmlEscapeNewlineChars = new char[7]
  {
    '<',
    '>',
    '&',
    '\'',
    '"',
    '\r',
    '\n'
  };

  private static string RemoveInvalidXmlChars(string text)
  {
    if (string.IsNullOrEmpty(text))
      return string.Empty;
    for (int index = 0; index < text.Length; ++index)
    {
      if (!XmlConvert.IsXmlChar(text[index]))
        return XmlHelper.CreateValidXmlString(text);
    }
    return text;
  }

  private static string CreateValidXmlString(string text)
  {
    StringBuilder stringBuilder = new StringBuilder(text.Length);
    for (int index = 0; index < text.Length; ++index)
    {
      char ch = text[index];
      if (XmlConvert.IsXmlChar(ch))
        stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  internal static string EscapeXmlString(string text, bool xmlEncodeNewlines, StringBuilder result = null)
  {
    if (result == null && XmlHelper.SmallAndNoEscapeNeeded(text, xmlEncodeNewlines))
      return text;
    StringBuilder stringBuilder = result ?? new StringBuilder(text.Length);
    for (int index = 0; index < text.Length; ++index)
    {
      switch (text[index])
      {
        case '\n':
          if (xmlEncodeNewlines)
          {
            stringBuilder.Append("&#10;");
            break;
          }
          stringBuilder.Append(text[index]);
          break;
        case '\r':
          if (xmlEncodeNewlines)
          {
            stringBuilder.Append("&#13;");
            break;
          }
          stringBuilder.Append(text[index]);
          break;
        case '"':
          stringBuilder.Append("&quot;");
          break;
        case '&':
          stringBuilder.Append("&amp;");
          break;
        case '\'':
          stringBuilder.Append("&apos;");
          break;
        case '<':
          stringBuilder.Append("&lt;");
          break;
        case '>':
          stringBuilder.Append("&gt;");
          break;
        default:
          stringBuilder.Append(text[index]);
          break;
      }
    }
    return result != null ? (string) null : stringBuilder.ToString();
  }

  private static bool SmallAndNoEscapeNeeded(string text, bool xmlEncodeNewlines)
  {
    return text.Length < 4096 /*0x1000*/ && text.IndexOfAny(xmlEncodeNewlines ? XmlHelper.XmlEscapeNewlineChars : XmlHelper.XmlEscapeChars) < 0;
  }

  internal static string XmlConvertToStringSafe(object value)
  {
    return XmlHelper.XmlConvertToString(value, true);
  }

  internal static string XmlConvertToString(object value)
  {
    return XmlHelper.XmlConvertToString(value, false);
  }

  internal static string XmlConvertToElementName(string xmlElementName, bool allowNamespace)
  {
    if (string.IsNullOrEmpty(xmlElementName))
      return xmlElementName;
    xmlElementName = XmlHelper.RemoveInvalidXmlChars(xmlElementName);
    StringBuilder sb = (StringBuilder) null;
    for (int index = 0; index < xmlElementName.Length; ++index)
    {
      char c = xmlElementName[index];
      if (char.IsLetter(c))
      {
        sb?.Append(c);
      }
      else
      {
        bool flag = false;
        switch (c)
        {
          case '-':
          case '.':
          case '0':
          case '1':
          case '2':
          case '3':
          case '4':
          case '5':
          case '6':
          case '7':
          case '8':
          case '9':
            if (index != 0)
            {
              if (sb != null)
              {
                sb.Append(c);
                continue;
              }
              continue;
            }
            flag = true;
            break;
          case ':':
            if (index != 0 & allowNamespace)
            {
              allowNamespace = false;
              if (sb != null)
              {
                sb.Append(c);
                continue;
              }
              continue;
            }
            break;
          case '_':
            if (sb != null)
            {
              sb.Append(c);
              continue;
            }
            continue;
        }
        if (sb == null)
          sb = CreateStringBuilder(index);
        sb.Append('_');
        if (flag)
          sb.Append(c);
      }
    }
    if (sb != null)
      sb.TrimRight();
    return sb?.ToString() ?? xmlElementName;

    StringBuilder CreateStringBuilder(int i)
    {
      StringBuilder stringBuilder = new StringBuilder(xmlElementName.Length);
      if (i > 0)
        stringBuilder.Append(xmlElementName, 0, i);
      return stringBuilder;
    }
  }

  private static string XmlConvertToString(object value, bool safeConversion)
  {
    try
    {
      IConvertible convertible = value as IConvertible;
      TypeCode objTypeCode = value == null ? TypeCode.Empty : (convertible != null ? convertible.GetTypeCode() : TypeCode.Object);
      return objTypeCode != TypeCode.Object ? XmlHelper.XmlConvertToString(convertible, objTypeCode, safeConversion) : XmlHelper.XmlConvertToStringInvariant(value, safeConversion);
    }
    catch
    {
      return safeConversion ? "" : (string) null;
    }
  }

  private static string XmlConvertToStringInvariant(object value, bool safeConversion)
  {
    try
    {
      string text = Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
      return safeConversion ? XmlHelper.RemoveInvalidXmlChars(text) : text;
    }
    catch
    {
      return safeConversion ? "" : (string) null;
    }
  }

  internal static string XmlConvertToString(
    IConvertible value,
    TypeCode objTypeCode,
    bool safeConversion = false)
  {
    if (objTypeCode == TypeCode.Empty || value == null)
      return "null";
    switch (objTypeCode)
    {
      case TypeCode.Boolean:
        return XmlConvert.ToString(value.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.Char:
        return XmlConvert.ToString(value.ToChar((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.SByte:
        return XmlConvert.ToString(value.ToSByte((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.Byte:
        return XmlConvert.ToString(value.ToByte((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.Int16:
        return XmlConvert.ToString(value.ToInt16((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.UInt16:
        return XmlConvert.ToString(value.ToUInt16((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.Int32:
        return XmlConvert.ToString(value.ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.UInt32:
        return XmlConvert.ToString(value.ToUInt32((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.Int64:
        return XmlConvert.ToString(value.ToInt64((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.UInt64:
        return XmlConvert.ToString(value.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.Single:
        float single = value.ToSingle((IFormatProvider) CultureInfo.InvariantCulture);
        return !float.IsInfinity(single) ? XmlConvert.ToString(single) : Convert.ToString(single, (IFormatProvider) CultureInfo.InvariantCulture);
      case TypeCode.Double:
        double d = value.ToDouble((IFormatProvider) CultureInfo.InvariantCulture);
        return !double.IsInfinity(d) ? XmlConvert.ToString(d) : Convert.ToString(d, (IFormatProvider) CultureInfo.InvariantCulture);
      case TypeCode.Decimal:
        return XmlConvert.ToString(value.ToDecimal((IFormatProvider) CultureInfo.InvariantCulture));
      case TypeCode.DateTime:
        return XmlConvert.ToString(value.ToDateTime((IFormatProvider) CultureInfo.InvariantCulture), XmlDateTimeSerializationMode.Utc);
      case TypeCode.String:
        return !safeConversion ? value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : XmlHelper.RemoveInvalidXmlChars(value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      default:
        return XmlHelper.XmlConvertToStringInvariant((object) value, safeConversion);
    }
  }

  public static void WriteAttributeSafeString(
    this XmlWriter writer,
    string prefix,
    string localName,
    string ns,
    string value)
  {
    writer.WriteAttributeString(XmlHelper.RemoveInvalidXmlChars(prefix), XmlHelper.RemoveInvalidXmlChars(localName), XmlHelper.RemoveInvalidXmlChars(ns), XmlHelper.RemoveInvalidXmlChars(value));
  }

  public static void WriteAttributeSafeString(
    this XmlWriter writer,
    string localName,
    string value)
  {
    writer.WriteAttributeString(XmlHelper.RemoveInvalidXmlChars(localName), XmlHelper.RemoveInvalidXmlChars(value));
  }

  public static void WriteElementSafeString(
    this XmlWriter writer,
    string prefix,
    string localName,
    string ns,
    string value)
  {
    writer.WriteElementString(XmlHelper.RemoveInvalidXmlChars(prefix), XmlHelper.RemoveInvalidXmlChars(localName), XmlHelper.RemoveInvalidXmlChars(ns), XmlHelper.RemoveInvalidXmlChars(value));
  }

  public static void WriteSafeCData(this XmlWriter writer, string text)
  {
    writer.WriteCData(XmlHelper.RemoveInvalidXmlChars(text));
  }
}
