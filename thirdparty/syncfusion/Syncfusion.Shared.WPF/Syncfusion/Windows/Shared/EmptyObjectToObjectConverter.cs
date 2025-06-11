// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.EmptyObjectToObjectConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class EmptyObjectToObjectConverter : DependencyObject, IValueConverter
{
  public static readonly DependencyProperty NotEmptyValueProperty = DependencyProperty.Register(nameof (NotEmptyValue), typeof (object), typeof (EmptyObjectToObjectConverter), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EmptyValueProperty = DependencyProperty.Register(nameof (EmptyValue), typeof (object), typeof (EmptyObjectToObjectConverter), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CanConvertToTargetTypeProperty = DependencyProperty.Register(nameof (CanConvertToTargetType), typeof (bool), typeof (EmptyObjectToObjectConverter), new PropertyMetadata((object) true));

  public object NotEmptyValue
  {
    get => this.GetValue(EmptyObjectToObjectConverter.NotEmptyValueProperty);
    set => this.SetValue(EmptyObjectToObjectConverter.NotEmptyValueProperty, value);
  }

  public object EmptyValue
  {
    get => this.GetValue(EmptyObjectToObjectConverter.EmptyValueProperty);
    set => this.SetValue(EmptyObjectToObjectConverter.EmptyValueProperty, value);
  }

  public bool CanConvertToTargetType
  {
    get => (bool) this.GetValue(EmptyObjectToObjectConverter.CanConvertToTargetTypeProperty);
    set
    {
      this.SetValue(EmptyObjectToObjectConverter.CanConvertToTargetTypeProperty, (object) value);
    }
  }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    bool flag = this.CheckValueIsEmpty(value);
    if (TypeConverterHelper.ChanngeTypeToBool(parameter))
      flag = !flag;
    if (this.CanConvertToTargetType)
      return TypeConverterHelper.ChangeType(flag ? this.EmptyValue : this.NotEmptyValue, targetType);
    return !flag ? this.NotEmptyValue : this.EmptyValue;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }

  protected virtual bool CheckValueIsEmpty(object value) => value == null;
}
