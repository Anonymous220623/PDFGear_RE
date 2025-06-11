// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintQueueNameToVisibilityConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintQueueNameToVisibilityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    string str = (string) value;
    if (str == parameter.ToString())
      return (object) Visibility.Visible;
    return str != "Fax" && parameter.ToString() == "Printer" ? (object) Visibility.Visible : (object) Visibility.Collapsed;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
