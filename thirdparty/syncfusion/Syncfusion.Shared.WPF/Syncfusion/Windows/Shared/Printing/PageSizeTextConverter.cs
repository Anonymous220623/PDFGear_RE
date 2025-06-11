// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PageSizeTextConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PageSizeTextConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values[0] is Size)
    {
      Size size = (Size) values[0];
      switch (values[1].ToString())
      {
        case "Inches":
          double num1 = 0.393700787 * 0.026458333333;
          return (object) $"{(object) Math.Round(size.Width * num1, 2)}'' x {(object) Math.Round(size.Height * num1, 2)}''";
        case "Centimeters":
          double num2 = 0.0264583333;
          return (object) $"{(object) Math.Round(size.Width * num2, 2)} cm  x {(object) Math.Round(size.Height * num2, 2)} cm ";
      }
    }
    return (object) null;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
