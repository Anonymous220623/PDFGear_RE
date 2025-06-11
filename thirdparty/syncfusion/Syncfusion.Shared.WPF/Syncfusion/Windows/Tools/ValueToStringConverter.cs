// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.ValueToStringConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools;

[ValueConversion(typeof (double), typeof (float))]
public class ValueToStringConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (parameter == null)
      return (object) ((float) value).ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture);
    switch (parameter.ToString())
    {
      case "S":
        return (object) (float) ((double) (float) value * 100.0);
      case "V":
        return (object) (float) ((double) (float) value * 100.0);
      default:
        return (object) ((float) value).ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      if (parameter == null)
        return (object) (float) System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture);
      switch (parameter.ToString())
      {
        case "S":
          return (object) (System.Convert.ToDouble(value) / 100.0);
        case "V":
          return (object) (System.Convert.ToDouble(value) / 100.0);
        default:
          return (object) (float) System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }
    catch
    {
      value = (object) 1f;
      return (object) (float) System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
