// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.BoolToObjectConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class BoolToObjectConverter : DependencyObject, IValueConverter
{
  public static readonly DependencyProperty TrueValueProperty = DependencyProperty.Register(nameof (TrueValue), typeof (object), typeof (BoolToObjectConverter), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty FalseValueProperty = DependencyProperty.Register(nameof (FalseValue), typeof (object), typeof (BoolToObjectConverter), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CanConvertToTargetTypeProperty = DependencyProperty.Register(nameof (CanConvertToTargetType), typeof (bool), typeof (BoolToObjectConverter), new PropertyMetadata((object) true));

  public object TrueValue
  {
    get => this.GetValue(BoolToObjectConverter.TrueValueProperty);
    set => this.SetValue(BoolToObjectConverter.TrueValueProperty, value);
  }

  public object FalseValue
  {
    get => this.GetValue(BoolToObjectConverter.FalseValueProperty);
    set => this.SetValue(BoolToObjectConverter.FalseValueProperty, value);
  }

  public bool CanConvertToTargetType
  {
    get => (bool) this.GetValue(BoolToObjectConverter.CanConvertToTargetTypeProperty);
    set => this.SetValue(BoolToObjectConverter.CanConvertToTargetTypeProperty, (object) value);
  }

  public virtual object Convert(
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

  public virtual object ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    bool flag = object.Equals(value, TypeConverterHelper.ChangeType(this.TrueValue, value.GetType()));
    if (TypeConverterHelper.ChanngeTypeToBool(parameter))
      flag = !flag;
    return (object) flag;
  }
}
