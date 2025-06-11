// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.HorizontalContentToTextAlignmentConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class HorizontalContentToTextAlignmentConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    TextAlignment textAlignment;
    switch ((int) value)
    {
      case 0:
        textAlignment = TextAlignment.Left;
        break;
      case 1:
        textAlignment = TextAlignment.Center;
        break;
      case 2:
        textAlignment = TextAlignment.Right;
        break;
      default:
        textAlignment = TextAlignment.Justify;
        break;
    }
    return (object) textAlignment;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    HorizontalAlignment horizontalAlignment;
    switch ((int) value)
    {
      case 0:
        horizontalAlignment = HorizontalAlignment.Left;
        break;
      case 1:
        horizontalAlignment = HorizontalAlignment.Right;
        break;
      case 2:
        horizontalAlignment = HorizontalAlignment.Center;
        break;
      default:
        horizontalAlignment = HorizontalAlignment.Stretch;
        break;
    }
    return (object) horizontalAlignment;
  }
}
