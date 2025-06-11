// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.BrushToTextConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools;

public class BrushToTextConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    Brush brush = (Brush) values[0];
    Color color = (Color) values[1];
    if ((GradientBrushDisplayMode) values[2] != GradientBrushDisplayMode.Default || !(brush is GradientBrush))
      return (object) SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, ColorEdit.SuchColor(color)[0]);
    return brush is LinearGradientBrush ? (object) SharedLocalizationResourceAccessor.Instance.GetString("LinearGradient") : (object) SharedLocalizationResourceAccessor.Instance.GetString("RadialGradient");
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    return (object[]) null;
  }
}
