// Decompiled with JetBrains decompiler
// Type: NLog.Common.ConversionHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;

#nullable disable
namespace NLog.Common;

public static class ConversionHelpers
{
  public static bool TryParseEnum<TEnum>(
    string inputValue,
    out TEnum resultValue,
    TEnum defaultValue = default (TEnum))
    where TEnum : struct
  {
    if (ConversionHelpers.TryParseEnum<TEnum>(inputValue, true, out resultValue))
      return true;
    resultValue = defaultValue;
    return false;
  }

  internal static bool TryParseEnum(string inputValue, Type enumType, out object resultValue)
  {
    if (StringHelpers.IsNullOrWhiteSpace(inputValue))
    {
      resultValue = (object) null;
      return false;
    }
    try
    {
      resultValue = Enum.Parse(enumType, inputValue, true);
      return true;
    }
    catch (ArgumentException ex)
    {
      resultValue = (object) null;
      return false;
    }
  }

  internal static bool TryParseEnum<TEnum>(
    string inputValue,
    bool ignoreCase,
    out TEnum resultValue)
    where TEnum : struct
  {
    if (!StringHelpers.IsNullOrWhiteSpace(inputValue))
      return Enum.TryParse<TEnum>(inputValue, ignoreCase, out resultValue);
    resultValue = default (TEnum);
    return false;
  }

  private static bool TryParseEnum_net3<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
  {
    Type type = typeof (TEnum);
    if (!type.IsEnum())
      throw new ArgumentException($"Type '{type.FullName}' is not an enum");
    if (StringHelpers.IsNullOrWhiteSpace(value))
    {
      result = default (TEnum);
      return false;
    }
    try
    {
      result = (TEnum) Enum.Parse(type, value, ignoreCase);
      return true;
    }
    catch (Exception ex)
    {
      result = default (TEnum);
      return false;
    }
  }
}
