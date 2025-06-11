// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ObjectToBoolConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class ObjectToBoolConverter : BoolToObjectConverter
{
  public override object Convert(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    bool flag = !this.CanConvertToTargetType ? value == this.TrueValue : object.Equals(value, TypeConverterHelper.ChangeType(this.TrueValue, value.GetType()));
    if (TypeConverterHelper.ChanngeTypeToBool(parameter))
      flag = !flag;
    return (object) flag;
  }

  public override object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    bool flag1 = value is bool flag2 && flag2;
    if (TypeConverterHelper.ChanngeTypeToBool(parameter))
      flag1 = !flag1;
    if (this.CanConvertToTargetType)
      return TypeConverterHelper.ChangeType(flag1 ? this.TrueValue : this.FalseValue, targetType);
    return !flag1 ? this.FalseValue : this.TrueValue;
  }
}
