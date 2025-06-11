// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.ColorToValueConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools;

[ValueConversion(typeof (double), typeof (Color))]
public class ColorToValueConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    Color color = (Color) value;
    return (object) (((int) color.A).ToString("X") + ((int) color.R).ToString("X") + ((int) color.G).ToString("X") + ((int) color.B).ToString("X"));
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new Exception("The method or operation is not implemented.");
  }
}
