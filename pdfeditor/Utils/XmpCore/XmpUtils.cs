// Decompiled with JetBrains decompiler
// Type: XmpCore.XmpUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Text;
using XmpCore.Impl;
using XmpCore.Options;

#nullable disable
namespace XmpCore;

public static class XmpUtils
{
  public static string CatenateArrayItems(
    IXmpMeta xmp,
    string schemaNs,
    string arrayName,
    string separator,
    string quotes,
    bool allowCommas)
  {
    return XmpCore.Impl.XmpUtils.CatenateArrayItems(xmp, schemaNs, arrayName, separator, quotes, allowCommas);
  }

  public static void SeparateArrayItems(
    IXmpMeta xmp,
    string schemaNs,
    string arrayName,
    string catedStr,
    PropertyOptions arrayOptions,
    bool preserveCommas)
  {
    XmpCore.Impl.XmpUtils.SeparateArrayItems(xmp, schemaNs, arrayName, catedStr, arrayOptions, preserveCommas);
  }

  public static void RemoveProperties(
    IXmpMeta xmp,
    string schemaNs,
    string propName,
    bool doAllProperties,
    bool includeAliases)
  {
    XmpCore.Impl.XmpUtils.RemoveProperties(xmp, schemaNs, propName, doAllProperties, includeAliases);
  }

  public static void AppendProperties(
    IXmpMeta source,
    IXmpMeta dest,
    bool doAllProperties,
    bool replaceOldValues,
    bool deleteEmptyValues = false)
  {
    XmpCore.Impl.XmpUtils.AppendProperties(source, dest, doAllProperties, replaceOldValues, deleteEmptyValues);
  }

  public static bool ConvertToBoolean(string value)
  {
    if (string.IsNullOrEmpty(value))
      throw new XmpException("Empty convert-string", XmpErrorCode.BadValue);
    int result;
    if (int.TryParse(value, out result))
      return result != 0;
    return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "t", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "on", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
  }

  public static string ConvertFromBoolean(bool value) => !value ? "False" : "True";

  public static int ConvertToInteger(string rawValue)
  {
    if (string.IsNullOrEmpty(rawValue))
      throw new XmpException("Empty convert-string", XmpErrorCode.BadValue);
    int result;
    if ((rawValue.StartsWith("0x") ? (int.TryParse(rawValue.Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result) ? 1 : 0) : (int.TryParse(rawValue, out result) ? 1 : 0)) == 0)
      throw new XmpException("Invalid integer string", XmpErrorCode.BadValue);
    return result;
  }

  public static string ConvertFromInteger(int value) => value.ToString();

  public static long ConvertToLong(string rawValue)
  {
    if (string.IsNullOrEmpty(rawValue))
      throw new XmpException("Empty convert-string", XmpErrorCode.BadValue);
    long result;
    if ((rawValue.StartsWith("0x") ? (long.TryParse(rawValue.Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result) ? 1 : 0) : (long.TryParse(rawValue, out result) ? 1 : 0)) == 0)
      throw new XmpException("Invalid long string", XmpErrorCode.BadValue);
    return result;
  }

  public static string ConvertFromLong(long value) => value.ToString();

  public static double ConvertToDouble(string rawValue)
  {
    if (string.IsNullOrEmpty(rawValue))
      throw new XmpException("Empty convert-string", XmpErrorCode.BadValue);
    double result;
    if (!double.TryParse(rawValue, out result))
      throw new XmpException("Invalid double string", XmpErrorCode.BadValue);
    return result;
  }

  public static string ConvertFromDouble(double value) => value.ToString();

  public static IXmpDateTime ConvertToDate(string rawValue)
  {
    return !string.IsNullOrEmpty(rawValue) ? Iso8601Converter.Parse(rawValue) : throw new XmpException("Empty convert-string", XmpErrorCode.BadValue);
  }

  public static string ConvertFromDate(IXmpDateTime value) => Iso8601Converter.Render(value);

  public static string EncodeBase64(byte[] buffer) => Convert.ToBase64String(buffer);

  public static byte[] DecodeBase64(string base64String)
  {
    try
    {
      return Convert.FromBase64String(base64String);
    }
    catch (Exception ex)
    {
      throw new XmpException("Invalid base64 string", XmpErrorCode.BadValue, ex);
    }
  }

  public static void PackageForJPEG(
    IXmpMeta origXMP,
    StringBuilder stdStr,
    StringBuilder extStr,
    StringBuilder digestStr)
  {
    XmpCore.Impl.XmpUtils.PackageForJPEG(origXMP, stdStr, extStr, digestStr);
  }

  public static void MergeFromJPEG(IXmpMeta fullXMP, IXmpMeta extendedXMP)
  {
    XmpCore.Impl.XmpUtils.MergeFromJPEG(fullXMP, extendedXMP);
  }

  public static void ApplyTemplate(
    IXmpMeta workingXMP,
    IXmpMeta templateXMP,
    TemplateOptions options)
  {
    XmpCore.Impl.XmpUtils.ApplyTemplate(workingXMP, templateXMP, options);
  }

  public static void DuplicateSubtree(
    IXmpMeta source,
    IXmpMeta dest,
    string sourceNS,
    string sourceRoot,
    string destNS,
    string destRoot,
    PropertyOptions options)
  {
    XmpCore.Impl.XmpUtils.DuplicateSubtree(source, dest, sourceNS, sourceRoot, destNS, destRoot, options);
  }
}
