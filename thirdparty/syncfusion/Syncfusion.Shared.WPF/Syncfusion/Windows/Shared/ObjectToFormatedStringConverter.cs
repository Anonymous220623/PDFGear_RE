// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ObjectToFormatedStringConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

[ValueConversion(typeof (object), typeof (string))]
public class ObjectToFormatedStringConverter : IValueConverter
{
  private const string DEFAULT_FORMAT = "{0}";

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string str = parameter as string;
    if (targetType != typeof (object) && targetType != typeof (string))
      throw new ArgumentException("Target type is invalid. Valid type is string.", nameof (targetType));
    return (object) string.Format((IFormatProvider) culture, str == null ? "{0}" : str, value);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException("Backward conversion is not supported.");
  }
}
