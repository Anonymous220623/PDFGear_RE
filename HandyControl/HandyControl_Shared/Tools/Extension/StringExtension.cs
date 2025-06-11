// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.StringExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.ComponentModel;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class StringExtension
{
  public static T Value<T>(this string input)
  {
    try
    {
      return (T) TypeDescriptor.GetConverter(typeof (T)).ConvertFromString(input);
    }
    catch
    {
      return default (T);
    }
  }

  public static object Value(this string input, Type type)
  {
    try
    {
      return TypeDescriptor.GetConverter(type).ConvertFromString(input);
    }
    catch
    {
      return (object) null;
    }
  }
}
