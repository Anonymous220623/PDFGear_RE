// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TypeConverterHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal static class TypeConverterHelper
{
  internal static bool ChanngeTypeToBool(object parameter)
  {
    bool result = false;
    return parameter != null && bool.TryParse(parameter.ToString(), out result) ? result : result;
  }

  public static object ChangeType(object value, Type type)
  {
    return TypeConverterHelper.ChangeType(value, type, (IFormatProvider) null);
  }

  public static object ChangeType(object value, Type type, IFormatProvider provider)
  {
    if (type == (Type) null)
      return value;
    if (value == null)
      return (object) null;
    if (type == typeof (ImageSource))
      return value;
    TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
    if (converter != null && converter.CanConvertTo(type))
      return converter.ConvertTo(value, type);
    if (value is DBNull)
      return (object) DBNull.Value;
    return type.IsEnum ? Enum.Parse(type, Convert.ToString(value)) : Convert.ChangeType(value, type, provider);
  }
}
