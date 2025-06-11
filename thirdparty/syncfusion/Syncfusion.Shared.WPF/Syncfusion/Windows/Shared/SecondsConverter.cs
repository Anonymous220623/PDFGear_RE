// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SecondsConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
[ValueConversion(typeof (DateTime), typeof (int))]
public class SecondsConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (((DateTime) value).Second * 6);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) null;
  }
}
