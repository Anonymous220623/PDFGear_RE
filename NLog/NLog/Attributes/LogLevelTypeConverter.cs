// Decompiled with JetBrains decompiler
// Type: NLog.Attributes.LogLevelTypeConverter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace NLog.Attributes;

public class LogLevelTypeConverter : TypeConverter
{
  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (string) || LogLevelTypeConverter.IsNumericType(sourceType) || base.CanConvertFrom(context, sourceType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    if (value?.GetType() == typeof (string))
      return (object) NLog.LogLevel.FromString(value.ToString());
    return LogLevelTypeConverter.IsNumericType(value?.GetType()) ? (object) NLog.LogLevel.FromOrdinal(Convert.ToInt32(value)) : base.ConvertFrom(context, culture, value);
  }

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (string) || LogLevelTypeConverter.IsNumericType(destinationType) || base.CanConvertTo(context, destinationType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (value is NLog.LogLevel logLevel)
    {
      if (destinationType == typeof (string))
        return (object) logLevel.ToString();
      if (LogLevelTypeConverter.IsNumericType(destinationType))
        return Convert.ChangeType((object) logLevel.Ordinal, destinationType, (IFormatProvider) culture);
    }
    return base.ConvertTo(context, culture, value, destinationType);
  }

  private static bool IsNumericType(Type sourceType)
  {
    return sourceType == typeof (int) || sourceType == typeof (uint) || sourceType == typeof (long) || sourceType == typeof (ulong) || sourceType == typeof (short) || sourceType == typeof (ushort);
  }
}
