// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ClipConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class ClipConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    return values[0] is double && values[1] is double && values.Length > 0 ? (object) new Rect(0.0, 0.0, (double) values[0], (double) values[1]) : (object) new Rect(0.0, 0.0, double.MaxValue, double.MaxValue);
  }

  public object[] ConvertBack(
    object value,
    Type[] targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
