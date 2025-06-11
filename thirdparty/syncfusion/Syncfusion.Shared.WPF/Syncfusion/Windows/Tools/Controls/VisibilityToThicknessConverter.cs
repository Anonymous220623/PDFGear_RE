// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.VisibilityToThicknessConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class VisibilityToThicknessConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (ScrollBarVisibility) value == ScrollBarVisibility.Disabled ? (object) new Thickness(0.0) : (object) new Thickness(0.0, 0.0, 0.0, 1.0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
