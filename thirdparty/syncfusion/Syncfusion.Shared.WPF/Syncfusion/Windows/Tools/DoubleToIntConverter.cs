// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.DoubleToIntConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools;

[DesignTimeVisible(false)]
[ValueConversion(typeof (double), typeof (int))]
public class DoubleToIntConverter : IValueConverter
{
  private const double Duration = 200.0;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    double num = 200.0 * (double) value;
    return (object) ((double) int.MaxValue > num ? (int) num : int.MaxValue);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new Exception("The method or operation is not implemented.");
  }
}
