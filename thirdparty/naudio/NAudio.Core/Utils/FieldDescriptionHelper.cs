// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.FieldDescriptionHelper
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Reflection;

#nullable disable
namespace NAudio.Utils;

public static class FieldDescriptionHelper
{
  public static string Describe(Type t, Guid guid)
  {
    foreach (FieldInfo field in t.GetFields(BindingFlags.Static | BindingFlags.Public))
    {
      if (field.IsPublic && field.IsStatic && field.FieldType == typeof (Guid) && (Guid) field.GetValue((object) null) == guid)
      {
        foreach (object customAttribute in field.GetCustomAttributes(false))
        {
          if (customAttribute is FieldDescriptionAttribute descriptionAttribute)
            return descriptionAttribute.Description;
        }
        return field.Name;
      }
    }
    return guid.ToString();
  }
}
