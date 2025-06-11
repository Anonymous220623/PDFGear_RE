// Decompiled with JetBrains decompiler
// Type: NLog.Config.PropertyTypeConverter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;

#nullable disable
namespace NLog.Config;

internal class PropertyTypeConverter : IPropertyTypeConverter
{
  public object Convert(
    object propertyValue,
    Type propertyType,
    string format,
    IFormatProvider formatProvider)
  {
    if (!PropertyTypeConverter.NeedToConvert(propertyValue, propertyType))
      return propertyValue;
    Type underlyingType = Nullable.GetUnderlyingType(propertyType);
    Type type = underlyingType;
    if ((object) type == null)
      type = propertyType;
    Type conversionType = type;
    if (propertyValue is string str)
    {
      string propertyString;
      propertyValue = (object) (propertyString = str.Trim());
      if (underlyingType != (Type) null && StringHelpers.IsNullOrWhiteSpace(propertyString))
        return (object) null;
      if (conversionType == typeof (DateTime))
        return PropertyTypeConverter.ConvertDateTime(format, formatProvider, propertyString);
      if (conversionType == typeof (DateTimeOffset))
        return PropertyTypeConverter.ConvertDateTimeOffset(format, formatProvider, propertyString);
      if (conversionType == typeof (TimeSpan))
        return PropertyTypeConverter.ConvertTimeSpan(format, formatProvider, propertyString);
      if (conversionType == typeof (Guid))
        return PropertyTypeConverter.ConvertGuid(format, propertyString);
    }
    else if (!string.IsNullOrEmpty(format) && propertyValue is IFormattable formattable)
      propertyValue = (object) formattable.ToString(format, formatProvider);
    return System.Convert.ChangeType(propertyValue, conversionType, formatProvider);
  }

  private static bool NeedToConvert(object propertyValue, Type propertyType)
  {
    return propertyType != (Type) null && propertyValue != null && propertyValue.GetType() != propertyType && propertyType != typeof (object);
  }

  private static object ConvertGuid(string format, string propertyString)
  {
    return (object) (string.IsNullOrEmpty(format) ? Guid.Parse(propertyString) : Guid.ParseExact(propertyString, format));
  }

  private static object ConvertTimeSpan(
    string format,
    IFormatProvider formatProvider,
    string propertyString)
  {
    return !string.IsNullOrEmpty(format) ? (object) TimeSpan.ParseExact(propertyString, format, formatProvider) : (object) TimeSpan.Parse(propertyString, formatProvider);
  }

  private static object ConvertDateTimeOffset(
    string format,
    IFormatProvider formatProvider,
    string propertyString)
  {
    return !string.IsNullOrEmpty(format) ? (object) DateTimeOffset.ParseExact(propertyString, format, formatProvider) : (object) DateTimeOffset.Parse(propertyString, formatProvider);
  }

  private static object ConvertDateTime(
    string format,
    IFormatProvider formatProvider,
    string propertyString)
  {
    return !string.IsNullOrEmpty(format) ? (object) DateTime.ParseExact(propertyString, format, formatProvider) : (object) DateTime.Parse(propertyString, formatProvider);
  }
}
