// Decompiled with JetBrains decompiler
// Type: Ionic.EnumUtil
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Ionic;

internal sealed class EnumUtil
{
  private EnumUtil()
  {
  }

  internal static string GetDescription(Enum value)
  {
    DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
    return customAttributes.Length != 0 ? customAttributes[0].Description : value.ToString();
  }

  internal static object Parse(Type enumType, string stringRepresentation)
  {
    return EnumUtil.Parse(enumType, stringRepresentation, false);
  }

  internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
  {
    if (ignoreCase)
      stringRepresentation = stringRepresentation.ToLower();
    foreach (Enum @enum in Enum.GetValues(enumType))
    {
      string str = EnumUtil.GetDescription(@enum);
      if (ignoreCase)
        str = str.ToLower();
      if (str == stringRepresentation)
        return (object) @enum;
    }
    return Enum.Parse(enumType, stringRepresentation, ignoreCase);
  }
}
