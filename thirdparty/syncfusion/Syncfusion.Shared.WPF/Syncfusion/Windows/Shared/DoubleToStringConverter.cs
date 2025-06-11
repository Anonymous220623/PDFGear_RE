// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DoubleToStringConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

[ValueConversion(typeof (double), typeof (string))]
public class DoubleToStringConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string format = "0.00";
    if (targetType != typeof (string))
      throw new ArgumentException("Target type is invalid. Valid type is string.", nameof (targetType));
    if (parameter != null && parameter.GetType() == typeof (string))
      format = (string) parameter;
    if (culture == null)
      culture = CultureInfo.CurrentCulture;
    return (object) ((double) value).ToString(format, (IFormatProvider) culture.DateTimeFormat);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (culture == null)
      culture = CultureInfo.CurrentCulture;
    return (object) double.Parse((string) value, (IFormatProvider) culture.DateTimeFormat);
  }
}
