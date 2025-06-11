// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.RangedFloatToStringConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools;

public class RangedFloatToStringConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) Math.Round(System.Convert.ToDouble(value) * (double) byte.MaxValue, 0).ToString();
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      if (value.ToString() != "")
        return (object) (float) (System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture) / (double) byte.MaxValue);
      value = (object) (int) byte.MaxValue;
      return (object) (float) (System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture) / (double) byte.MaxValue);
    }
    catch
    {
      value = (object) (int) byte.MaxValue;
      return (object) (float) (System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture) / (double) byte.MaxValue);
    }
  }
}
