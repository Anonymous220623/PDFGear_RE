// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.EnumDataProvider
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Data;

public class EnumDataProvider : ObjectDataProvider
{
  private Type _type;
  private bool _useAttributes;

  public EnumDataProvider() => this.MethodName = "GetValues";

  public Type Type
  {
    get => this._type;
    set
    {
      this._type = value;
      this.MethodParameters.Add((object) value);
      this.ObjectType = typeof (Enum);
    }
  }

  public bool UseAttributes
  {
    get => this._useAttributes;
    set
    {
      this._useAttributes = value;
      this.ObjectType = value ? typeof (EnumDataProvider) : typeof (Enum);
    }
  }

  public static IEnumerable<EnumItem> GetValues(Type enumType)
  {
    if (enumType == (Type) null)
      throw new ArgumentNullException(nameof (enumType));
    List<EnumItem> values = new List<EnumItem>();
    foreach (Enum @enum in Enum.GetValues(enumType))
    {
      FieldInfo field = enumType.GetField(@enum.ToString());
      if (!(field == (FieldInfo) null))
      {
        object[] customAttributes1 = field.GetCustomAttributes(typeof (BrowsableAttribute), true);
        if (!(customAttributes1 is BrowsableAttribute[] browsableAttributeArray) || customAttributes1.Length <= 0 || browsableAttributeArray[0].Browsable)
        {
          object[] customAttributes2 = field.GetCustomAttributes(typeof (DescriptionAttribute), true);
          if (customAttributes2 is DescriptionAttribute[] descriptionAttributeArray && customAttributes2.Length > 0)
            values.Add(new EnumItem()
            {
              Description = descriptionAttributeArray[0].Description,
              Value = @enum
            });
        }
      }
    }
    return (IEnumerable<EnumItem>) values;
  }
}
