// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.GradientBrushToVisibilityConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools;

public class GradientBrushToVisibilityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return parameter.ToString() == "Linear" ? ((Brush) value is RadialGradientBrush ? (object) Visibility.Collapsed : (object) Visibility.Visible) : ((Brush) value is RadialGradientBrush ? (object) Visibility.Visible : (object) Visibility.Collapsed);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) null;
  }
}
