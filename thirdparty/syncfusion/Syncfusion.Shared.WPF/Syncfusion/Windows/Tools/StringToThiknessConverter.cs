// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.StringToThiknessConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools;

[DesignTimeVisible(false)]
[ValueConversion(typeof (string), typeof (Thickness))]
public class StringToThiknessConverter : IValueConverter
{
  private const string Up = "Up";
  private const string Down = "Down";

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string paramName = value.ToString();
    if ("Up" == paramName)
      return (object) new Thickness(0.0, 0.0, 1.0, 0.0);
    if ("Down" == paramName)
      return (object) new Thickness(1.0, 0.0, 0.0, 0.0);
    throw new ArgumentException("Incorrect argument", paramName);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) null;
  }
}
