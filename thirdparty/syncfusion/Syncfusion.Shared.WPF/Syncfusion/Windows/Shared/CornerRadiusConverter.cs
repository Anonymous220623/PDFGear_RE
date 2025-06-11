// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CornerRadiusConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class CornerRadiusConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    CornerRadius cornerRadius1 = (CornerRadius) value;
    CornerRadius cornerRadius2 = new CornerRadius();
    if ((string) parameter == "Top")
    {
      cornerRadius2.TopLeft = cornerRadius1.TopLeft;
      cornerRadius2.TopRight = cornerRadius1.TopRight;
      cornerRadius2.BottomLeft = 0.0;
      cornerRadius2.BottomRight = 0.0;
      return (object) cornerRadius2;
    }
    cornerRadius2.TopLeft = 0.0;
    cornerRadius2.TopRight = 0.0;
    cornerRadius2.BottomLeft = cornerRadius1.BottomLeft;
    cornerRadius2.BottomRight = cornerRadius1.BottomRight;
    return (object) cornerRadius2;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) string.Empty;
  }
}
