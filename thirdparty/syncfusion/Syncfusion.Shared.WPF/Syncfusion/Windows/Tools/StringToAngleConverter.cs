// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.StringToAngleConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools;

[ValueConversion(typeof (string), typeof (double))]
[DesignTimeVisible(false)]
public class StringToAngleConverter : IValueConverter
{
  private const string Up = "Up";
  private const string Down = "Down";

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string paramName = value.ToString();
    if ("Up" == paramName)
      return (object) 0.0;
    if ("Down" == paramName)
      return (object) 180.0;
    throw new ArgumentException("incorrect argument", paramName);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) null;
  }
}
