// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ProgressConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class ProgressConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values[0] == null || values[2] == null)
      return (object) 0.0;
    double result1;
    double.TryParse(values[0].ToString(), out result1);
    double result2;
    double.TryParse(values[1].ToString(), out result2);
    double result3;
    double.TryParse(values[2].ToString(), out result3);
    double result4 = 0.0;
    if (values.Length > 3)
      double.TryParse(values[3].ToString(), out result4);
    result4 = double.IsNegativeInfinity(result4) ? 0.0 : result4;
    double num = double.IsPositiveInfinity(result3) ? 0.0 : result3;
    return (object) ((result1 - result4) * (result2 / (num - result4)) / result2);
  }

  public object[] ConvertBack(
    object value,
    Type[] targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
